using System;
using UnityEngine;

public class GenericHealth : MonoBehaviour
{
    Entity parent;
    public float max_hp;
    [HideInInspector] public float current_hp;

    public void init(Entity parent)
    {
        this.parent = parent;

        current_hp = max_hp;
        update_health();
	}

    void update_health()
    {
        if (current_hp <= 0) parent.destroy();
    }

    public void take_damage(float f)
    {
        if (f < 0) Debug.LogWarning("take_damage only takes in positive numbers (" + f + ")");
        current_hp -= f;
        update_health();
    }

    public void gain_hp(float f)
    {
        if (f < 0) Debug.LogWarning("gain_hp only takes in positive numbers (" + f + ")");
        current_hp += f;
        update_health();
    }
}
