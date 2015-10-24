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
    [HideInInspector] public EnemyGun gun;
    [HideInInspector] public GenericHealth health;

    [HideInInspector] public GeneralAIState general_ai_state = GeneralAIState.NONE;

    private void Start()
    {
        //make sure the enemy contains at least one ai logic
        if (GetComponents<AILogicBase>().Length == 0)
        {
            Debug.LogError("No AILogicBase component can be found on enemy gameobject (" + name + ")");
        }

        //get EnemyGun component on gun object and init it
        GameObject gun_obj = transform.parent.FindChild("gun").gameObject;
        if (gun_obj == null) Debug.LogError("Child 'gun' of enemy parent cannot be found");
        gun = gun_obj.GetComponent<EnemyGun>();
        gun.init(this);

        health = GetComponent<GenericHealth>();
        health.init(this);
    }

    private void Update()
    {
        gun.update();
    }

    public override void destroy()
    {
        Debug.Log("destroyed!");
    }

    private EnemyType get_type() { return type; }

    public void set_type(EnemyType _type)
    {
        if (type != EnemyType.UNKNOWN) Debug.LogError("Cannot initiate an enemy of a different type");
        else type = _type;
    }
}
