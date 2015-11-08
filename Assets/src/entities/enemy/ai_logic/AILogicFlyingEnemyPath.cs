using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AILogicFlyingEnemyPath : AILogicBase
{
    Enemy parent;
    AILogicFlyingEnemy ai_parent;

    [HideInInspector] public List<WaypointNode> path = null;
    [HideInInspector] public WaypointNode next_node = null;
    [HideInInspector] public WaypointNode current_node = null;
    [HideInInspector] public int current_path_index;

    float recalc_path_timer = 0;
    const int RECALC_PATH_MS = 500;
    public float path_find_timeout_ms = 1.25f;

	void Start() {
        ai_parent = GetComponent<AILogicFlyingEnemy>();

        ai_parent.pos = Map.grid.grid_to_world(Map.grid.world_to_grid(transform.position));
        ai_parent.pos.z = -10;

        current_node = Map.grid.get_node(Map.grid.world_to_grid(ai_parent.pos));
    }

    public void get_next_path_node()
    {
        if (path != null && path.Count != 0 && current_path_index < path.Count) next_node = path[current_path_index];
    }

    public void calc_move_angle()
    {
        ai_parent.move_angle = Mathf.Atan2(next_node.world_pos.y - ai_parent.pos.y, next_node.world_pos.x - ai_parent.pos.x);
    }

    public void recalc_path()
    {
        if (next_node != null) current_node = next_node;
        Vector2 dest = Map.grid.world_to_grid(Entities.player.pos);
        List<WaypointNode> temp_path = Map.grid.find_path(Map.grid.find_neighbour_node(current_node), 
                                                          Map.grid.find_neighbour_node(Map.grid.get_node(dest.x, dest.y)), path_find_timeout_ms);

        if (temp_path.Count >= 2) path = temp_path;
        else if (path != null) path.Clear();

        if (path != null && path.Count >= 2)
        {
            current_path_index = 1;
            get_next_path_node();
        }
    }

    public void try_recalc_path() {
        if (recalc_path_timer >= RECALC_PATH_MS / 1000.0f)
        {
            recalc_path_timer = 0;
            recalc_path();
        }
    }

    public bool has_valid_path()
    {
        return path != null && path.Count >= 2 && next_node != null;
    }

    public void check_arrived_next_node()
    {
        float dist = Mathf.Sqrt(Mathf.Pow(ai_parent.pos.x - next_node.world_pos.x, 2) + Mathf.Pow(ai_parent.pos.y - next_node.world_pos.y, 2));
        if (dist <= 1)
        {
            try_recalc_path();

            if (path != null && path.Count != 0 && current_path_index + 1 < path.Count)
            {
                ++current_path_index;
                next_node = path[current_path_index];
            }
        }
        if (dist >= 4.0f) try_recalc_path();
    }

    void Update()
    {
        recalc_path_timer += Time.deltaTime;

        current_node = Map.grid.get_node(Map.grid.world_to_grid(ai_parent.pos));
	}
}
