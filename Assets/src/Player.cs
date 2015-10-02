using UnityEngine;
using System.Collections;

public class Player : Singleton<Player> {

    public WaypointNode current_node = null;
    public Vector3 pos;

	void Start() {
        pos = WaypointGrid.instance.grid_to_world(WaypointGrid.instance.world_to_grid(transform.position));
        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));
	}

    void Update() {
        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));
	}
}
