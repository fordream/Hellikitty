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
    private EnemyType type = EnemyType.UNKNOWN;

    [HideInInspector] public bool facing_right = false;
    [HideInInspector] public EnemyGun gun;

    [HideInInspector] public GeneralAIState general_ai_state = GeneralAIState.NONE;

    private void Start()
    {
        if (GetComponents<AILogicBase>().Length == 0)
        {
            Debug.LogError("No AILogicBase component can be found on enemy gameobject (" + name + ")");
        }
    }

    private EnemyType get_type() { return type; }

    public void set_type(EnemyType _type)
    {
        if (type != EnemyType.UNKNOWN) Debug.LogError("Cannot initiate an enemy of a different type");
        else type = _type;
    }
}
