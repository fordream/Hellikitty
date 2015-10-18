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
    public GeneralAIState general_ai_state = GeneralAIState.NONE;
    public bool facing_right = false;

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
