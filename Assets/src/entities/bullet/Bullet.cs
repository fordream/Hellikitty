using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bullet : BulletLogicBase
{
    [HideInInspector] public Entity entity;
    private static List<Bullet> bullet_list = new List<Bullet>();

    public LayerMask player_owner_colliders;
    public LayerMask enemy_owner_colliders;
    [HideInInspector] public LayerMask colliders;

    public void init_base()
    {
        colliders = entity == Entities.player ? player_owner_colliders : enemy_owner_colliders;
    }

    public T add_logic<T>() where T : Component
    {
        return gameObject.AddComponent<T>();
    }

    public static T spawn<T>(Entity entity, Vector2 pos, float depth = -20) where T : Bullet
    {
        string prefab_path = "bullets/";
        if (typeof(T) == typeof(BasicBullet)) prefab_path += "basic_bullet";
        else if (typeof(T) == typeof(RailgunBullet)) prefab_path += "railgun_bullet";
        else if (typeof(T) == typeof(GrenadeBullet)) prefab_path += "grenade_bullet";
        else Debug.LogError("Cannot spawn bullet of type " + typeof(T));

        GameObject bullet_prefab = (GameObject)Resources.Load(prefab_path);
        if (bullet_prefab == null) Debug.LogError("Could not find bullet prefab at path (" + prefab_path + ")");

        GameObject bullet_obj = GameObject.Instantiate(bullet_prefab);
        bullet_obj.transform.position = new Vector3(pos.x, pos.y, depth);

        Bullet bullet = bullet_obj.GetComponent<Bullet>();
        bullet.entity = entity;
        bullet_list.Add(bullet);

        return bullet_obj.GetComponent<T>();
    }
}
