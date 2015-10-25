using System;
using UnityEngine;

public enum WeaponType
{
    NONE, 
    PISTOL, 
    RAILGUN, 
    GRENADE_LAUNCHER
};

public class WeaponBase : MonoBehaviour
{
    [HideInInspector] public Vector3 pos;
    [HideInInspector] public float angle;
    [HideInInspector] public WeaponType equipped_weapon_type;
    public WeaponType[] starting_weapon_types;

    private float rate_timer;

    public void init_weapon()
    {
        if (starting_weapon_types.Length == 0) Debug.LogError("Starting weapon types must contain at least one weapon (in editor)");
        equipped_weapon_type = starting_weapon_types[0];
    }

    public void update_weapon_motion(GameObject base_obj, Vector2 target)
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

    public void update_weapon_logic(bool activate)
    {
        rate_timer += Time.deltaTime;
        if (activate && rate_timer >= .1f)
        {
            rate_timer = 0;
            spawn(equipped_weapon_type);
        }
    }

    public void spawn(WeaponType type)
    {
        if (type == WeaponType.PISTOL) Bullet.spawn<BasicBullet>(pos).init(angle);
        if (type == WeaponType.RAILGUN) Bullet.spawn<RailgunBullet>(pos).init(angle);
        if (type == WeaponType.GRENADE_LAUNCHER) Bullet.spawn<BasicBullet>(pos).init(angle);
    }
}
