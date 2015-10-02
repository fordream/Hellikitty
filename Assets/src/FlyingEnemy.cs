using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemy : MonoBehaviour {

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

	void Start() {
        pos = WaypointGrid.instance.grid_to_world(WaypointGrid.instance.world_to_grid(transform.position));
        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));

        update_path();
        current_path_index = 0;
        get_next_path_node();
        update_angle();
    }
    
    bool update_path()
    {
        Vector2 dest = WaypointGrid.instance.world_to_grid(GameObject.Find("goal").transform.position);
        List<WaypointNode> temp_path = WaypointGrid.instance.find_path((int)current_node.grid_pos.x, (int)current_node.grid_pos.y,
                                                                       (int)dest.x, (int)dest.y);

        if (temp_path.Count >= 2)
        {
            path = temp_path;
            return true;
        }
        return false;
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
        update_angle();
		accel.x += Mathf.Cos(angle) * speed_multiplier;
		accel.y += Mathf.Sin(angle) * speed_multiplier;
        accel.x *= friction;
        accel.y *= friction;
        accel.x = Mathf.Clamp(accel.x, -max_accel, max_accel);
        accel.y = Mathf.Clamp(accel.y, -max_accel, max_accel);
        pos.x += accel.x;
        pos.y += accel.y;

        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));

        float dist = Mathf.Sqrt(Mathf.Pow(pos.x - next_node.world_pos.x, 2) + Mathf.Pow(pos.y - next_node.world_pos.y, 2));
        if (dist < .2f)
        {
            //++current_path_index;
            //get_next_path_node();

			current_node = next_node;
            if (update_path())
			{
				current_path_index = 1;
                get_next_path_node();
            }
        }
        dist = Mathf.Sqrt(Mathf.Pow(pos.x - Player.instance.pos.x, 2) + Mathf.Pow(pos.y - Player.instance.pos.y, 2));
        if (dist < 4)
        {

        }

        transform.position = pos;
	}
}
