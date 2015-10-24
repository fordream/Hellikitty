using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerGun))]
[RequireComponent(typeof(GrapplingHook))]
[RequireComponent(typeof(GenericHealth))]
public class Player : Entity
{

    [HideInInspector] public Controller2D controller;
    [HideInInspector] public PlayerMovement player_movement;
    [HideInInspector] public PlayerGun gun;
    [HideInInspector] public GrapplingHook grappling_hook;
    [HideInInspector] public GenericHealth health;

    [HideInInspector] public WaypointNode current_node = null;
    [HideInInspector] public Vector3 pos;

    public LayerMask colliders;
    bool grappling = false;
    float grapple_angle = 0;

    public void init()
    {
        pos = Map.grid.grid_to_world(Map.grid.world_to_grid(transform.position));
        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));

        controller = GetComponent<Controller2D>();
        controller.init();
        player_movement = GetComponent<PlayerMovement>();
        player_movement.init();
        gun = transform.parent.FindChild("gun").GetComponent<PlayerGun>();
        gun.init();
        grappling_hook = GetComponent<GrapplingHook>();
        grappling_hook.init();
        health = GetComponent<GenericHealth>();
        health.init(this);
    }

    void update_scale()
    {
        Vector3 scale = transform.localScale;
        float scale_x = Mathf.Abs(scale.x);
        if (player_movement.velocity.x > 0) scale_x = -scale_x;
        scale.x = -scale_x;
        transform.localScale = scale;
    }

    public override void destroy()
    {
        Debug.Log("destroyed!");
    }

    void Update()
    {
        grappling_hook.update();
        if (grappling_hook.grapple_state == GrapplingHook.GrappleState.NONE)
        {
            update_scale();
            player_movement.update();
        }
        pos = transform.position;

        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));
    }
}
