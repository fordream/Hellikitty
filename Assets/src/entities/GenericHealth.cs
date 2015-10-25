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
    private float total_bar_width;
    private SpriteRenderer srenderer;

    public void init(Entity parent)
    {
        this.parent = parent;

        srenderer = GetComponent<SpriteRenderer>();
        create_bars();

        current_hp = max_hp;
        update_health();
	}

    public void update()
    {
        health_bar.transform.position = new Vector3(transform.position.x - (total_bar_width / 2.0f), 
                                                    transform.position.y + (srenderer.bounds.size.y / 2.0f), 
                                                    -20);
    }

    private void create_bars()
    {
        GameObject bar1_asset = (GameObject)Resources.Load("misc/health_bar1");
        GameObject bar2_asset = (GameObject)Resources.Load("misc/health_bar2");
        GameObject grey_bar_asset = (GameObject)Resources.Load("misc/health_bar_grey");

        int num_bars = 8;
        Vector3 pos = Vector3.zero;
        health_bar = new GameObject();
        health_bar.name = "health_bar(" + name + ")";
        health_bar.transform.parent = parent.transform.parent.transform;

        for (int n = 0; n < num_bars; ++n)
        {
            pos.z = 0;
            GameObject bar = Instantiate(n % 2 == 0 ? bar1_asset : bar2_asset);
            bar.transform.position = pos;
            bar.transform.parent = health_bar.transform;
            pos.x += .13f;
            bars.Add(bar);

            pos.z = 1;
            GameObject grey_bar = Instantiate(grey_bar_asset);
            grey_bar.transform.position = pos;
            grey_bar.transform.parent = health_bar.transform;
        }
        total_bar_width = pos.x;
    }

    private void update_health()
    {
        if (current_hp <= 0) { parent.destroy(); return; }

        int bars_removed = (int)((current_hp / max_hp) * bars.Count);
        for (int n = 0; n < bars.Count; ++n)
        {
            bars[n].SetActive(n < bars_removed);
        }
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
