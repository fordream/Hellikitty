using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(RemovalScheduler))]
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public RemovalScheduler removal;

    static List<Bullet> bullet_list = new List<Bullet>();

    void Awake()
    {
        removal = GetComponent<RemovalScheduler>();
    }

    public T add_logic<T>() where T : Component
    {
        return gameObject.AddComponent<T>();
    }

    public static Bullet spawn(Vector2 pos, float depth = -20)
    {
        GameObject bullet_obj = GameObject.Instantiate((GameObject)Resources.Load("bullet"));
        bullet_obj.transform.position = new Vector3(pos.x, pos.y, depth);

        Bullet bullet = bullet_obj.GetComponent<Bullet>();
        bullet_list.Add(bullet);

        return bullet;
    }

    public static void update_all()
    {
        for (int n = 0; n < bullet_list.Count; ++n)
        {
            if (bullet_list[n].removal.is_scheduled())
            {
                bullet_list.RemoveAt(n);
                --n;
            }
        }
    }
}
