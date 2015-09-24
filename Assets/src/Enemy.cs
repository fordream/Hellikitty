using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    List<WaypointNode> path;
    WaypointNode next_node;
    int current_path_index;
    Vector3 pos;
    float angle;
    Vector2 accel;

	void Start() {
        pos = WaypointGrid.instance.grid_to_world(WaypointGrid.instance.world_to_grid(transform.position));

        Vector2 dest = GameObject.Find("goal").transform.position;
        update_path();
        foreach (WaypointNode n in path)
        {
            n.debug_draw_path = true;
        }
        current_path_index = 0;
        get_next_path_node();
        update_angle();
    }
    
    bool update_path()
    {
        Vector2 dest = GameObject.Find("goal").transform.position;
        List<WaypointNode> temp_path = WaypointGrid.instance.find_path(transform.position, dest);
        if (temp_path.Count != 0)
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

    void Update() {
        update_angle();
        accel.x += Mathf.Cos(angle) / 200.0f;
        accel.y += Mathf.Sin(angle) / 200.0f;
        accel.x *= .99f;
        accel.y *= .99f;
        accel.x = Mathf.Clamp(accel.x, -.05f, .05f);
        accel.y = Mathf.Clamp(accel.y, -.05f, .05f);
        pos.x += accel.x;
        pos.y += accel.y;

        float dist = Mathf.Sqrt(Mathf.Pow(pos.x - next_node.world_pos.x, 2) + Mathf.Pow(pos.y - next_node.world_pos.y, 2));
        if (dist < .5f)
        {
            //++current_path_index;
            //get_next_path_node();

            Vector2 dest = GameObject.Find("goal").transform.position;
            if (update_path())
            {
                current_path_index = 0;
                get_next_path_node();
            }
        }

        transform.position = pos;
	}
}
