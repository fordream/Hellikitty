using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemyPath : MonoBehaviour
{
    FlyingEnemy parent;

    [HideInInspector] public List<WaypointNode> path = null;
    [HideInInspector] public WaypointNode next_node = null;
    [HideInInspector] public WaypointNode current_node = null;
    [HideInInspector] public int current_path_index;

    System.Diagnostics.Stopwatch calc_path_watch = new System.Diagnostics.Stopwatch();
    const int RECALC_PATH_MS = 500;
    public float path_find_timeout_ms = 1.25f;

	void Start() {
        parent = GetComponent<FlyingEnemy>();

        parent.pos = Map.grid.grid_to_world(Map.grid.world_to_grid(transform.position));
        parent.pos.z = -10;

        current_node = Map.grid.get_node(Map.grid.world_to_grid(parent.pos));
    }

    public void get_next_path_node()
    {
        next_node = path[current_path_index];
    }

    public void calc_move_angle()
    {
        parent.move_angle = Mathf.Atan2(next_node.world_pos.y - parent.pos.y, next_node.world_pos.x - parent.pos.x);
    }

    public void recalc_path()
    {
        if (next_node != null) current_node = next_node;
        Vector2 dest = Map.grid.world_to_grid(Entities.player.pos);
        List<WaypointNode> temp_path = Map.grid.find_path(Map.grid.find_neighbour_node(current_node), 
                                                          Map.grid.find_neighbour_node(Map.grid.get_node(dest.x, dest.y)), path_find_timeout_ms);

        if (temp_path.Count >= 2) path = temp_path;
        else path.Clear();

        if (path.Count >= 2)
        {
            current_path_index = 1;
            get_next_path_node();
        }
    }

    public void try_recalc_path() {
        if (calc_path_watch.ElapsedMilliseconds >= RECALC_PATH_MS)
        {
            calc_path_watch.Reset();
            calc_path_watch.Start();
            recalc_path();
        }
        else if (!calc_path_watch.IsRunning)
        {
            calc_path_watch.Reset();
            calc_path_watch.Start();
        }
    }

    public bool has_valid_path()
    {
        return path != null && path.Count >= 2 && next_node != null;
    }

    public void check_arrived_next_node()
    {
        float dist = Mathf.Sqrt(Mathf.Pow(parent.pos.x - next_node.world_pos.x, 2) + Mathf.Pow(parent.pos.y - next_node.world_pos.y, 2));
        if (dist < .2f)
        {
            try_recalc_path();

            ++current_path_index;
            get_next_path_node();
        }
        if (dist >= 4.0f) try_recalc_path();
    }

    void Update()
    {
        current_node = Map.grid.get_node(Map.grid.world_to_grid(parent.pos));
	}
}
