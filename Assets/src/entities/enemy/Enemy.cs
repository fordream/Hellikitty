﻿using System;
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
    [HideInInspector] public EnemyWeapon weapon;
    [HideInInspector] public GenericHealth health;

    [HideInInspector] public GeneralAIState general_ai_state = GeneralAIState.NONE;

    private void Start()
    {
        //make sure the enemy contains at least one ai logic
        if (GetComponents<AILogicBase>().Length == 0)
        {
            Debug.LogError("No AILogicBase component can be found on enemy gameobject (" + name + ")");
        }

        //get EnemyWeapon component on weapon object and init it
        GameObject weapon_obj = transform.parent.FindChild("weapon").gameObject;
        if (weapon_obj == null) Debug.LogError("Child 'weapon' of enemy parent cannot be found");
        weapon = weapon_obj.GetComponent<EnemyWeapon>();
        weapon.init(this);

        health = GetComponent<GenericHealth>();
        health.init(this);
    }

    private void Update()
    {
        weapon.update();
        health.update();
    }

    public override void destroy()
    {
        Destroy(transform.parent.gameObject);
    }

    private EnemyType get_type() { return type; }

    public void set_type(EnemyType _type)
    {
        if (type != EnemyType.UNKNOWN) Debug.LogError("Cannot initiate an enemy of a different type");
        else type = _type;
    }
}
