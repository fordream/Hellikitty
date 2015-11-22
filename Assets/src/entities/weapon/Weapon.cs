using System;
using UnityEngine;

public enum WeaponType
{
    NONE, 
    PISTOL, 
    RAILGUN, 
    GRENADE_LAUNCHER, 
    SHOTGUN,
    SNIPER,
    LASER,
    ROCKET,
    XRAY
};

public class Weapon : MonoBehaviour
{
    [HideInInspector] public Entity entity;
    [HideInInspector] public Vector3 pos;
    [HideInInspector] public float angle;
    [HideInInspector] public WeaponType type;

    [HideInInspector] public GameObject weapon_asset;
    [HideInInspector] public GameObject weapon_asset_instance;

    public float attack_rate_sec = 1.0f;
    public bool initially_equipped = false;
    public float aim_radius = 1.25f;

    private float rate_timer = 0;

    protected bool fire_ready = false;

    public void update(GameObject target_obj, Vector2 target_pos, bool can_fire)
    {
        GameObject wobj = weapon_asset_instance;

        Vector3 rota = wobj.transform.localEulerAngles;
        angle = Mathf.Atan2(target_pos.y - target_obj.transform.position.y, 
                            target_pos.x - target_obj.transform.position.x);
        angle *= target_obj.transform.localScale.x / target_obj.transform.localScale.x;
        rota.z = angle * (180.0f / Mathf.PI);
        wobj.transform.localEulerAngles = rota;

        Vector3 scale = wobj.transform.localScale;
        float scale_y = Mathf.Abs(scale.y);
        if (rota.z <= -90 || rota.z >= 90) scale_y = -scale_y;
        scale.y = scale_y;
        wobj.transform.localScale = scale;

        pos = target_obj.transform.position;
        pos.x += Mathf.Cos(angle) * aim_radius;
        pos.y += Mathf.Sin(angle) * aim_radius;
        wobj.transform.position = pos;

        fire_ready = false;
        rate_timer += Time.deltaTime;
        if (can_fire && rate_timer >= attack_rate_sec)
        {
            rate_timer = 0;
            fire_ready = true;
        }
    }
}
