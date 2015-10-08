using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : Singleton<Player> {

    Vector2 aimVec;
    Transform gun;

    public Controller2D controller;
    public PlayerMovement player_movement;

    public WaypointNode current_node = null;
    public Vector3 pos;

	void Start() {
        Debug.Log("player start");

        pos = WaypointGrid.instance.grid_to_world(WaypointGrid.instance.world_to_grid(transform.position));
        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));

        controller = GetComponent<Controller2D>();
        controller.init();
        player_movement = GetComponent<PlayerMovement>();
        player_movement.init();
    }

    void Update() {
        pos = transform.position;

        current_node = WaypointGrid.instance.get_node(WaypointGrid.instance.world_to_grid(pos));

        transform.position = pos;
	}
}
