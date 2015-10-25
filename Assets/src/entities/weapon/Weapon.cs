using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public Entity entity;
    [HideInInspector] public Vector3 pos;
    [HideInInspector] public float angle;
    public WeaponType type;
    public float attacks_per_ms = 0;

    private float rate_timer;

    public void update_motion(GameObject base_obj, Vector2 target)
    {
        Vector3 rota = transform.localEulerAngles;
        angle = Mathf.Atan2(target.y - base_obj.transform.position.y, 
                                  target.x - base_obj.transform.position.x);
        angle *= base_obj.transform.localScale.x / base_obj.transform.localScale.x;
        rota.z = angle * (180.0f / Mathf.PI);
        transform.localEulerAngles = rota;

        Vector3 scale = transform.localScale;
        float scale_y = Mathf.Abs(scale.y);
        if (rota.z <= -90 || rota.z >= 90) scale_y = -scale_y;
        scale.y = scale_y;
        transform.localScale = scale;

        pos = base_obj.transform.position;
        pos.x += Mathf.Cos(angle) * 1;
        pos.y += Mathf.Sin(angle) * 1;
        transform.position = pos;
    }

    public void update_logic(bool activate)
    {
        rate_timer += Time.deltaTime;
        if (activate && rate_timer >= attacks_per_ms / 1000.0f)
        {
            rate_timer = 0;
            spawn(type);
        }
    }

    public void spawn(WeaponType type)
    {
        if (type == WeaponType.PISTOL) Bullet.spawn<BasicBullet>(entity, pos).init(angle);
        if (type == WeaponType.RAILGUN) Bullet.spawn<RailgunBullet>(entity, pos).init(angle);
        if (type == WeaponType.GRENADE_LAUNCHER) Bullet.spawn<GrenadeBullet>(entity, pos).init(angle);
    }
}
