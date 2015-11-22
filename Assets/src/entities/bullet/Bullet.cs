using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BulletLogic;

public class Bullet : MonoBehaviour
{
    public static T spawn<T>(Entity entity, Vector2 pos, float depth = -20) where T : BulletLogicBase
    {
        string prefab_path = "";
        if (typeof(T) == typeof(BulletLogic.Asset.PistolAsset)) prefab_path = "pistol_bullet";
        else if (typeof(T) == typeof(BulletLogic.Asset.RailgunAsset)) prefab_path = "railgun_bullet";
        else if (typeof(T) == typeof(BulletLogic.Asset.GrenadeAsset)) prefab_path = "grenade_bullet";
        else if (typeof(T) == typeof(BulletLogic.Asset.ShotgunAsset)) prefab_path = "shotgun_bullet";
        else if (typeof(T) == typeof(BulletLogic.Asset.SniperAsset)) prefab_path = "sniper_bullet";
        else if (typeof(T) == typeof(BulletLogic.Asset.LaserAsset)) prefab_path = "lasermach_bullet";
        else if (typeof(T) == typeof(BulletLogic.Asset.XRayAsset)) prefab_path = "xray_bullet";

        GameObject bullet_obj;
        if (prefab_path != "")
        {
            prefab_path = "bullets/" + prefab_path;
            GameObject bullet_prefab = (GameObject)Resources.Load(prefab_path);
            if (bullet_prefab == null) Debug.LogError("Could not find bullet prefab at path (" + prefab_path + ")");
            bullet_obj = Instantiate(bullet_prefab);
        }else {
            bullet_obj = new GameObject();
            bullet_obj.AddComponent<T>();
        }

        bullet_obj.transform.position = new Vector3(pos.x, pos.y, depth);
        bullet_obj.GetComponent<T>().entity = entity;

        return bullet_obj.GetComponent<T>();
    }
}
