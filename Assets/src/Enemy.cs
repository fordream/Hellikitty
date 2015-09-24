using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    List<WaypointNode> path;
    WaypointNode next_node;
    int current_path_index;
    Vector3 pos;
    float angle;

	void Start() {
        pos = WaypointGrid.instance.grid_to_world(WaypointGrid.instance.world_to_grid(transform.position));

        Vector2 dest = GameObject.Find("goal").transform.position;
        path = WaypointGrid.instance.find_path(transform.position, dest);
        foreach (WaypointNode n in path)
        {
            n.debug_draw_path = true;
        }
        current_path_index = 0;
        get_next_path_node();
        update_angle();
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
        pos.x += Mathf.Cos(angle) / 40.0f;
        pos.y += Mathf.Sin(angle) / 40.0f;
        float dist = Mathf.Sqrt(Mathf.Pow(pos.x - next_node.world_pos.x, 2) + Mathf.Pow(pos.y - next_node.world_pos.y, 2));
        if (dist < .5f)
        {
            ++current_path_index;
            get_next_path_node();
        }

        transform.position = pos;
	}
}
