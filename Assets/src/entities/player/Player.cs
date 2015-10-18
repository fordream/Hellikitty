using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour {

    Vector2 aimVec;
    Transform gun;

    [HideInInspector] public Controller2D controller;
    [HideInInspector] public PlayerMovement player_movement;

    [HideInInspector] public WaypointNode current_node = null;
    [HideInInspector] public Vector3 pos;

    public void init()
    {
        pos = Map.grid.grid_to_world(Map.grid.world_to_grid(transform.position));
        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));

        controller = GetComponent<Controller2D>();
        controller.init();
        player_movement = GetComponent<PlayerMovement>();
        player_movement.init();
    }

    void Update() {
        Vector3 scale = transform.localScale;
        if (player_movement.velocity.x > 0) scale.x = 1;
        else scale.x = -1;
        transform.localScale = scale;

        player_movement.update();
        pos = transform.position;

        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "enemy")
        {
            Destroy(col.gameObject);
        }
    }
}
