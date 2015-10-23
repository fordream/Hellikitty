using System;
using UnityEngine;

public class AILogicBase : MonoBehaviour
{
    public Enemy get_enemy_parent()
    {
        return GetComponent<Enemy>();
    }
}
