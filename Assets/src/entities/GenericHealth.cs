using System;
using UnityEngine;

public class GenericHealth : MonoBehaviour
{
    public float max_hp;
    [HideInInspector] public float current_hp;

    public void init()
    {
        current_hp = max_hp;
        update_health();
	}

    void update_health()
    {

    }

    public void take_damage(float f)
    {
        current_hp -= f;
        update_health();
    }

    public void gain_hp(float f)
    {
        current_hp += f;
        update_health();
    }
}
