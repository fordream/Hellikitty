using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(GrapplingHook))]
[RequireComponent(typeof(GenericHealth))]
public class Player : Entity
{
    //components
    [HideInInspector] public Controller2D controller;
    [HideInInspector] public PlayerMovement player_movement;

    [HideInInspector] public PlayerWeaponControl weapon_control;
    [HideInInspector] public WeaponInventory weapon_inventory;

    [HideInInspector] public GrapplingHook grappling_hook;
    [HideInInspector] public GenericHealth health;

    //pos
    [HideInInspector] public WaypointNode current_node = null;
    [HideInInspector] public Vector3 pos;

    public LayerMask colliders;
    private bool grappling = false;
    private float grapple_angle = 0;

    public void init()
    {
        pos = Map.grid.grid_to_world(Map.grid.world_to_grid(transform.position));
        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));

        controller = GetComponent<Controller2D>();
        controller.init();
        player_movement = GetComponent<PlayerMovement>();
        player_movement.init();

        Transform weapon_obj = transform.parent.FindChild("weapon");
        if (weapon_obj == null) Debug.LogError("'weapon' object cannot be found in player parent's children");
        weapon_inventory = weapon_obj.GetComponent<WeaponInventory>();
        weapon_inventory.init();
        weapon_control = weapon_obj.GetComponent<PlayerWeaponControl>();
        weapon_control.init();

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
        health.update();
        weapon_control.update();

        pos = transform.position;

        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));
    }
}
