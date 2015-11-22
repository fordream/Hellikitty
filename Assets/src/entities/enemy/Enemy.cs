using System;
using UnityEngine;

public enum EnemyType
{
    UNKNOWN, 
    GROUND,
    FLYING
};

public enum GeneralAIState
{
    NONE, 
    WALKING, 
    SHOOTING
};

[RequireComponent(typeof(GenericHealth))]
public class Enemy : Entity
{
    private EnemyType type = EnemyType.UNKNOWN;

    [HideInInspector] public bool facing_right = false;

    //components
    [HideInInspector] public EnemyWeaponControl weapon_control;
    [HideInInspector] public WeaponInventory weapon_inventory;

    [HideInInspector] public GenericHealth health;

    [HideInInspector] public GeneralAIState general_ai_state = GeneralAIState.NONE;

    private void Start()
    {
        //make sure the enemy contains at least one ai logic
        if (GetComponents<AILogicBase>().Length == 0)
        {
            Debug.LogError("No AILogicBase component can be found on enemy gameobject (" + name + ")");
        }

        Transform weapon_obj = transform.parent.FindChild("weapon");
        if (weapon_obj == null) Debug.LogError("'weapon' object cannot be found in enemy parent's children");
        weapon_inventory = weapon_obj.GetComponent<WeaponInventory>();
        weapon_inventory.init(this);
        weapon_control = weapon_obj.GetComponent<EnemyWeaponControl>();
        weapon_control.init(this);

        health = GetComponent<GenericHealth>();
        health.init(this);
    }

    private void Update()
    {
        weapon_control.update();
        health.update();
    }

    public override void destroy()
    {
        Instantiate((GameObject)Resources.Load("particles/explosion"), 
                    new Vector3(transform.position.x, transform.position.y, -20), Quaternion.identity);

        Destroy(transform.parent.gameObject);
    }

    private EnemyType get_type() { return type; }

    public void set_type(EnemyType _type)
    {
        if (type != EnemyType.UNKNOWN) Debug.LogError("Cannot initiate an enemy of a different type");
        else type = _type;
    }
}
