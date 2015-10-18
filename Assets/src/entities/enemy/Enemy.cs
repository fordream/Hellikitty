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

public class Enemy : MonoBehaviour
{
    EnemyType type = EnemyType.UNKNOWN;
    [HideInInspector] public GeneralAIState general_ai_state = GeneralAIState.NONE;
    [HideInInspector] public bool facing_right = false;
    [HideInInspector] public EnemyGun gun;

    EnemyType get_type()
    {
        return type;
    }

    public void set_type(EnemyType _type)
    {
        if (type != EnemyType.UNKNOWN) Debug.LogError("Cannot initiate an enemy of a different type");
        else type = _type;
    }
}
