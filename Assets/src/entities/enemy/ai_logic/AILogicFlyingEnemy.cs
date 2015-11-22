using UnityEngine;

[RequireComponent(typeof(AILogicFlyingEnemyPath))]
public class AILogicFlyingEnemy : AILogicBase
{
    enum AIState {

        NONE, 
        MOVE_NEXT_NODE, 
        FIRE_AND_KEEP_DISTANCE
    };

    private Enemy parent;
    private AILogicFlyingEnemyPath pather;

    [HideInInspector] public Vector3 pos;
    [HideInInspector] public float move_angle;
    private Vector2 accel;

    private float friction = .9f;
    private float max_accel = 4.0f;
    private float speed_multiplier = 1.0f;

    private AIState ai_state = AIState.MOVE_NEXT_NODE;

    public float fire_radius = 8;
    public float move_away_radius = 5.5f;

    public LayerMask move_away_layers;
    public LayerMask sight_layers;

    private void Start()
    {
        parent = get_enemy_parent();
        parent.set_type(EnemyType.FLYING);

        pather = GetComponent<AILogicFlyingEnemyPath>();
    }

    bool is_player_in_sight(float radius)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
                                             new Vector2(Mathf.Cos(move_angle), Mathf.Sin(move_angle)),
                                             radius, sight_layers);

        return hit.transform == Entities.player.transform;
    }

    void update_scale()
    {
        float scale_x = Mathf.Abs(transform.localScale.x);
        if (accel.x > 0) scale_x = -scale_x;
        transform.localScale = new Vector3(scale_x, transform.localScale.y, transform.localScale.z);
        parent.facing_right = scale_x < 0;
    }

    void Update()
    {
        //disable collider so enemy doesn't raycast with itself
        GetComponent<Collider2D>().enabled = false;

        pos = transform.position;
        update_scale();

        float dist;
        RaycastHit2D hit;

        switch (ai_state) {
            case AIState.MOVE_NEXT_NODE:
                parent.general_ai_state = GeneralAIState.WALKING;

                if (!pather.has_valid_path()) { pather.try_recalc_path(); return; }

                pather.calc_move_angle();

                accel.x += Mathf.Cos(move_angle) * speed_multiplier;
                accel.y += Mathf.Sin(move_angle) * speed_multiplier;
                accel.x = Mathf.Clamp(accel.x, -max_accel, max_accel);
                accel.y = Mathf.Clamp(accel.y, -max_accel, max_accel);

                pather.check_arrived_next_node();

                move_angle = Mathf.Atan2(Entities.player.pos.y - pos.y, Entities.player.pos.x - pos.x);

                if (is_player_in_sight(move_away_radius)) ai_state = AIState.FIRE_AND_KEEP_DISTANCE;
                else if (is_player_in_sight(fire_radius)) parent.general_ai_state = GeneralAIState.SHOOTING;

                break;
            case AIState.FIRE_AND_KEEP_DISTANCE:
                parent.general_ai_state = GeneralAIState.SHOOTING;

                if (!is_player_in_sight(move_away_radius)) ai_state = AIState.MOVE_NEXT_NODE;

                dist = Mathf.Sqrt(Mathf.Pow(pos.x - Entities.player.pos.x, 2) + Mathf.Pow(pos.y - Entities.player.pos.y, 2));
                if (dist < move_away_radius)
                {
                    move_angle = Mathf.Atan2(Entities.player.pos.y - pos.y, Entities.player.pos.x - pos.x);
                    float best_angle = move_angle;
                    float closest_dist = -1;
                    const int num_casts = 12;
                    float sx = Mathf.Cos(move_angle) * fire_radius;
                    float sy = Mathf.Sin(move_angle) * fire_radius;

                    for (int n = 0; n < num_casts; ++n)
                    {
                        float target_angle = (Mathf.PI * 2.0f / num_casts) * (n + 1);

                        hit = Physics2D.Raycast(pos, new Vector2(Mathf.Cos(target_angle), Mathf.Sin(target_angle)),
                                                                fire_radius, move_away_layers);

                        float hit_dist = hit ? hit.distance : fire_radius;
                        float px = Mathf.Cos(target_angle) * hit_dist;
                        float py = Mathf.Sin(target_angle) * hit_dist;
                        dist = Mathf.Sqrt(Mathf.Pow(sx - px, 2) + Mathf.Pow(sy - py, 2));
                        if (dist > closest_dist)
                        {
                            closest_dist = dist;
                            best_angle = target_angle;
                        }
                    }

                    if (closest_dist != -1) {
                        move_angle = best_angle;
                        accel.x += Mathf.Cos(move_angle) * speed_multiplier * 2.0f;
                        accel.y += Mathf.Sin(move_angle) * speed_multiplier * 2.0f;
                        accel.x = Mathf.Clamp(accel.x, -max_accel, max_accel);
                        accel.y = Mathf.Clamp(accel.y, -max_accel, max_accel);
                    }
                }

                //flip scale towards player while shooting/moving away
                float scale_x = Mathf.Abs(transform.localScale.x);
                if (transform.position.x < Entities.player.transform.position.x) scale_x = -scale_x;
                transform.localScale = new Vector3(scale_x, transform.localScale.y, transform.localScale.z);
                parent.facing_right = transform.localScale.x < 0;

                break;
        }

        accel.x *= friction;
        accel.y *= friction;
        pos.x += accel.x * Time.deltaTime;
        pos.y += accel.y * Time.deltaTime;

        transform.position = pos;

        GetComponent<Collider2D>().enabled = true;
	}
}
