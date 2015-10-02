using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemy : MonoBehaviour {

    enum AIState {

        NONE, 
        CALCULTING_PATH, 
        MOVE_NEXT_NODE, 
        DISTANCE_SHOOTING
    };

    List<WaypointNode> path = null;
    WaypointNode next_node = null;
    WaypointNode current_node = null;
    int current_path_index;
    Vector3 pos;
    float angle;
    Vector2 accel;

    float friction = .96f;
	float max_accel = .1f;
    float speed_multiplier = .001f;

    AIState ai_state = AIState.CALCULTING_PATH;
    AIState prev_ai_state = AIState.NONE;
    System.Diagnostics.Stopwatch calc_path_watch = new System.Diagnostics.Stopwatch();
    const int RECALC_PATH_MS = 500;

	void Start() {
        pos = WaypointGrid.instance.grid_to_world(WaypointGrid.instance.world_to_grid(transform.position));
        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));
    }
    
    void update_path()
    {
        Vector2 dest = WaypointGrid.instance.world_to_grid(Player.instance.pos);
        List<WaypointNode> temp_path = WaypointGrid.instance.find_path((int)current_node.grid_pos.x, (int)current_node.grid_pos.y,
                                                                       (int)dest.x, (int)dest.y);

        if (temp_path.Count >= 2) path = temp_path;
        else path.Clear();
    }

    void get_next_path_node()
    {
        next_node = path[current_path_index];
    }

    void update_angle()
    {
        angle = Mathf.Atan2(next_node.world_pos.y - pos.y, next_node.world_pos.x - pos.x);
    }

    void Update()
    {
        float dist;
        AIState temp_prev_ai_state = ai_state;

        switch (ai_state) {
            case AIState.CALCULTING_PATH:
                if (ai_state != prev_ai_state || calc_path_watch.ElapsedMilliseconds >= RECALC_PATH_MS)
                {
                    calc_path_watch.Stop();

                    if (next_node != null) current_node = next_node;
                    update_path();
                    if (path.Count >= 2)
                    {
                        current_path_index = 1;
                        get_next_path_node();
                        ai_state = AIState.MOVE_NEXT_NODE;
                    }
                }else if (!calc_path_watch.IsRunning)
                {
                    calc_path_watch.Reset();
                    calc_path_watch.Start();
                }

                break;
            case AIState.MOVE_NEXT_NODE:
                update_angle();

		        accel.x += Mathf.Cos(angle) * speed_multiplier;
                accel.y += Mathf.Sin(angle) * speed_multiplier;
                accel.x = Mathf.Clamp(accel.x, -max_accel, max_accel);
                accel.y = Mathf.Clamp(accel.y, -max_accel, max_accel);

                dist = Mathf.Sqrt(Mathf.Pow(pos.x - next_node.world_pos.x, 2) + Mathf.Pow(pos.y - next_node.world_pos.y, 2));
                if (dist < .2f) ai_state = AIState.CALCULTING_PATH;

                break;
            case AIState.DISTANCE_SHOOTING:
                dist = Mathf.Sqrt(Mathf.Pow(pos.x - Player.instance.pos.x, 2) + Mathf.Pow(pos.y - Player.instance.pos.y, 2));
                if (dist < 4)
                {

                }
                break;
        }
        prev_ai_state = temp_prev_ai_state;

        accel.x *= friction;
        accel.y *= friction;
        pos.x += accel.x;
        pos.y += accel.y;

        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));

        transform.position = pos;
	}
}
