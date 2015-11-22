using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlatformCollision))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(GrapplingHook))]
[RequireComponent(typeof(GenericHealth))]
public class Player : Entity
{
    //components
    [HideInInspector] public PlatformCollision controller;
    [HideInInspector] public PlayerMovement player_movement;

    [HideInInspector] public PlayerWeaponControl weapon_control;
    [HideInInspector] public WeaponInventory weapon_inventory;

    [HideInInspector] public GrapplingHook grappling_hook;
    [HideInInspector] public GenericHealth health;

    //pos
    [HideInInspector] public WaypointNode current_node = null;
    [HideInInspector] public Vector3 pos;
    
    private bool grappling = false;
    private float grapple_angle = 0;

    public void init()
    {
        pos = Map.grid.grid_to_world(Map.grid.world_to_grid(transform.position));
        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));

        controller = GetComponent<PlatformCollision>();
        controller.init();
        player_movement = GetComponent<PlayerMovement>();
        player_movement.init();

        Transform weapon_obj = transform.parent.FindChild("weapon");
        if (weapon_obj == null) Debug.LogError("'weapon' object cannot be found in player parent's children");
        weapon_inventory = weapon_obj.GetComponent<WeaponInventory>();
        weapon_inventory.init(this);
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

    //Lachlan changed this to restart current level not the first level
    //ie it used to restart the game on death
    public override void destroy()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void Update()
    {
        //an addition by lachlan to skip levels as we please
        if (Input.GetKeyDown(KeyCode.P))
        {
            int p = Application.loadedLevel;
            Application.LoadLevel(p + 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            int o = Application.loadedLevel;
            Application.LoadLevel(o - 1);
        }

        grappling_hook.update();
        if (grappling_hook.grapple_state == GrapplingHook.GrappleState.NONE)
        {
            update_scale();
            player_movement.update();
        }
        health.update();
        weapon_control.update();

        pos = transform.position;

        float world_bounds = 4.0f;
        Vector3 top_left = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 bot_right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        if (pos.x < top_left.x - world_bounds || pos.x > bot_right.x + world_bounds ||
            pos.y < top_left.y - world_bounds || pos.y > bot_right.y + world_bounds)
        {
            //Lachlan changed this to restart current level not the first level
            //ie it used to restart the game on death (you should probably call void destroy here)
            Application.LoadLevel(Application.loadedLevel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) weapon_inventory.equip_weapon(weapon_inventory.weapons[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2)) weapon_inventory.equip_weapon(weapon_inventory.weapons[1]);
        if (Input.GetKeyDown(KeyCode.Alpha3)) weapon_inventory.equip_weapon(weapon_inventory.weapons[2]);
        if (Input.GetKeyDown(KeyCode.Alpha4)) weapon_inventory.equip_weapon(weapon_inventory.weapons[3]);
        if (Input.GetKeyDown(KeyCode.Alpha5)) weapon_inventory.equip_weapon(weapon_inventory.weapons[4]);
        if (Input.GetKeyDown(KeyCode.Alpha6)) weapon_inventory.equip_weapon(weapon_inventory.weapons[5]);

        current_node = Map.grid.get_node(Map.grid.world_to_grid(pos));
    }

    //lachlan additions
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "End")
        {
            int i = Application.loadedLevel;
            Application.LoadLevel(i + 1);
        }
        if (coll.gameObject.tag == "Music")
        {
            AudioSource aud = coll.gameObject.GetComponent<AudioSource>();
            aud.Play();//lachlan added playing of audio
            coll.gameObject.transform.position = new Vector2(-10, -10);
            //Destroy(coll.gameObject);
        }
    }
}
