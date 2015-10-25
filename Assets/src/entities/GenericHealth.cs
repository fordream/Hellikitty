using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericHealth : MonoBehaviour
{
    private Entity parent;
    public float max_hp;
    [HideInInspector] public float current_hp;

    private List<GameObject> bars = new List<GameObject>();
    private GameObject health_bar;

    public void init(Entity parent)
    {
        this.parent = parent;

        create_bars();

        current_hp = max_hp;
        update_health();
	}

    public void update()
    {
        health_bar.transform.position = transform.position;
    }

    private void create_bars()
    {
        GameObject bar1 = (GameObject)Resources.Load("misc/health_bar1");
        GameObject bar2 = (GameObject)Resources.Load("misc/health_bar2");

        int num_bars = 8;
        Vector3 pos = Vector3.zero;
        health_bar = new GameObject();
        health_bar.name = "health_bar(" + name + ")";

        for (int n = 0; n < num_bars; ++n)
        {
            GameObject bar = Instantiate(n % 2 == 0 ? bar1 : bar2);
            bar.transform.position = pos;
            bar.transform.parent = health_bar.transform;
            pos.x += .13f;
            bars.Add(bar);
        }
    }

    private void update_health()
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
