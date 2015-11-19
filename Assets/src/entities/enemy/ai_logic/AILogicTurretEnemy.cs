using UnityEngine;

public class AILogicTurretEnemy : AILogicBase
{
    enum AIState {

        NONE, 
        WALKING, 
        FIRE_AND_KEEP_DISTANCE
    };

    private Enemy parent;

    [HideInInspector] public Vector3 pos;
    private Vector2 accel;
    private bool moving_right = false;
    private Rigidbody2D rigid_body;
    private Vector2 velocity;

    private float friction = .9f;
    private float max_accel = 4.0f; //originally 4.0f
    private float speed_multiplier = 1.0f; //originally 1.0f

    private AIState ai_state = AIState.WALKING;
    private AIState prev_ai_state = AIState.NONE;

    public float fire_radius = 8;
    public float move_away_radius = 5.5f;

    public LayerMask move_away_layers;
    public LayerMask sight_layers;

    private Controller2D controller;

    private void Start()
    {
        parent = get_enemy_parent();
        parent.set_type(EnemyType.GROUND);

        rigid_body = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();
    }

    bool is_player_in_sight(float radius)
    {
        float angle = Mathf.Atan2(Entities.player.pos.y - pos.y, Entities.player.pos.x - pos.x);

        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
                                             radius, sight_layers);

        return hit.transform == Entities.player.transform;
    }

    void update_scale()
    {
        float scale_x = Mathf.Abs(transform.localScale.x);
        if (moving_right) scale_x = -scale_x;
        transform.localScale = new Vector3(scale_x, transform.localScale.y, transform.localScale.z);
        parent.facing_right = scale_x < 0;
    }

    void Update()
    {
        moving_right = Entities.player.transform.position.x > transform.position.x;
        update_scale();

        pos = transform.position;

        //disable collider so enemy doesn't raycast with itself
        GetComponent<Collider2D>().enabled = false;

        float dist;
        RaycastHit2D hit;

        switch (ai_state) {
            case AIState.WALKING:
                parent.general_ai_state = GeneralAIState.WALKING;

                if (moving_right) velocity.x = 0.0f; //originally 20.0f
                else velocity.x = -0.0f; //-originally 20.0f

                //velocity.x = Mathf.Clamp(velocity.x, -4, 4);

                if (is_player_in_sight(move_away_radius)) ai_state = AIState.FIRE_AND_KEEP_DISTANCE;
                if (is_player_in_sight(fire_radius)) parent.general_ai_state = GeneralAIState.SHOOTING;

                break;
            case AIState.FIRE_AND_KEEP_DISTANCE:
                parent.general_ai_state = GeneralAIState.SHOOTING;

                if (!is_player_in_sight(move_away_radius)) ai_state = AIState.WALKING;

                dist = Mathf.Sqrt(Mathf.Pow(pos.x - Entities.player.pos.x, 2) + Mathf.Pow(pos.y - Entities.player.pos.y, 2));
                if (dist < move_away_radius)
                {
                    float best_angle = Mathf.Atan2(Entities.player.pos.y - pos.y, Entities.player.pos.x - pos.x);
                    float closest_dist = -1;
                    const int num_casts = 12;
                    float sx = Mathf.Cos(best_angle) * fire_radius;
                    float sy = Mathf.Sin(best_angle) * fire_radius;

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
                        accel.x += Mathf.Cos(best_angle) * speed_multiplier * 2.0f;
                        accel.y += Mathf.Sin(best_angle) * speed_multiplier * 2.0f;
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

        velocity.y += -10.0f * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (controller.collisions.above || controller.collisions.below) velocity.y = 0;

        GetComponent<Collider2D>().enabled = true;
	}
}
