using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemy : Enemy
{

    enum AIState {

        NONE, 
        MOVE_NEXT_NODE, 
        FIRE_AND_KEEP_DISTANCE
    };

    FlyingEnemyPath pather;

    [HideInInspector] public Vector3 pos;
    [HideInInspector] public float move_angle;
    Vector2 accel;

    float friction = .96f;
    float max_accel = 4.0f;
    float speed_multiplier = 1.0f;

    AIState ai_state = AIState.MOVE_NEXT_NODE;
    AIState prev_ai_state = AIState.NONE;

    public float fire_radius = 8;
    public float move_away_radius = 5.5f;

	void Start() {
        set_type(EnemyType.FLYING);

        pather = GetComponent<FlyingEnemyPath>();
    }

    bool is_player_in_sight(float radius)
    {
        RaycastHit2D hit = Physics2D.Raycast(gun.transform.position, new Vector2(Mathf.Cos(move_angle), Mathf.Sin(move_angle)),
                                             radius, 1024 | Debug.config.grid_collidable_layers);

        return hit.transform == Entities.player.transform;
    }

    void update_scale()
    {
        Vector3 scale = transform.localScale;
        float scale_x = Mathf.Abs(scale.x);
        if (accel.x > 0) scale_x = -scale_x;
        scale.x = scale_x;
        transform.localScale = scale;
        facing_right = scale_x < 0;
    }

    void Update()
    {
        pos = transform.position;

        update_scale();

        float dist;
        AIState temp_prev_ai_state = ai_state;
        RaycastHit2D hit;

        Map.grid.reset_surface_offset(Debug.config.grid_terrain_offset);

        switch (ai_state) {
            case AIState.MOVE_NEXT_NODE:
                general_ai_state = GeneralAIState.WALKING;

                if (!pather.has_valid_path()) { pather.try_recalc_path(); return; }

                pather.calc_move_angle();

                accel.x += Mathf.Cos(move_angle) * speed_multiplier;
                accel.y += Mathf.Sin(move_angle) * speed_multiplier;
                accel.x = Mathf.Clamp(accel.x, -max_accel, max_accel);
                accel.y = Mathf.Clamp(accel.y, -max_accel, max_accel);

                pather.check_arrived_next_node();

                move_angle = Mathf.Atan2(Entities.player.pos.y - pos.y, Entities.player.pos.x - pos.x);

                if (is_player_in_sight(move_away_radius)) ai_state = AIState.FIRE_AND_KEEP_DISTANCE;
                else if (is_player_in_sight(fire_radius)) general_ai_state = GeneralAIState.SHOOTING;

                break;
            case AIState.FIRE_AND_KEEP_DISTANCE:
                general_ai_state = GeneralAIState.SHOOTING;

                if (!is_player_in_sight(move_away_radius)) ai_state = AIState.MOVE_NEXT_NODE;

                move_angle = Mathf.Atan2(Entities.player.pos.y - pos.y, Entities.player.pos.x - pos.x);

                dist = Mathf.Sqrt(Mathf.Pow(pos.x - Entities.player.pos.x, 2) + Mathf.Pow(pos.y - Entities.player.pos.y, 2));
                if (dist < move_away_radius)
                {
                    float best_angle = move_angle;
                    float closest_dist = -1;
                    const int num_casts = 8;
                    float sx = Mathf.Cos(move_angle) * fire_radius;
                    float sy = Mathf.Sin(move_angle) * fire_radius;

                    for (int n = 0; n < num_casts; ++n)
                    {
                        float target_angle = move_angle + (Mathf.PI * 2.0f / num_casts) * (n + 1);

                        hit = Physics2D.Raycast(pos, new Vector2(Mathf.Cos(target_angle), Mathf.Sin(target_angle)),
                                                                fire_radius, Debug.config.grid_collidable_layers);
                        if (!hit)
                        {
                            float px = Mathf.Cos(target_angle) * fire_radius;
                            float py = Mathf.Sin(target_angle) * fire_radius;
                            dist = Mathf.Sqrt(Mathf.Pow(sx - px, 2) + Mathf.Pow(sy - py, 2));
                            if (dist > closest_dist)
                            {
                                closest_dist = dist;
                                best_angle = target_angle;
                            }
                        }
                    }

                    if (closest_dist != -1) {
                        move_angle = best_angle;
                        accel.x += Mathf.Cos(move_angle) * speed_multiplier;
                        accel.y += Mathf.Sin(move_angle) * speed_multiplier;
                        accel.x = Mathf.Clamp(accel.x, -max_accel, max_accel);
                        accel.y = Mathf.Clamp(accel.y, -max_accel, max_accel);
                    }
                }

                break;
        }
        prev_ai_state = temp_prev_ai_state;

        accel.x *= friction;
        accel.y *= friction;
        pos.x += accel.x * Time.deltaTime;
        pos.y += accel.y * Time.deltaTime;

        transform.position = pos;

        Map.grid.reset_surface_offset(0);
	}
}
