using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]//lachlan added audiosource

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
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();//lachlan added playing of audio

        if (type == WeaponType.PISTOL) Bullet.spawn<BulletLogic.Asset.BasicAsset>(entity, pos).init(angle);
        if (type == WeaponType.RAILGUN) Bullet.spawn<BulletLogic.Asset.RailgunAsset>(entity, pos).init(angle);
        if (type == WeaponType.GRENADE_LAUNCHER) Bullet.spawn<BulletLogic.Asset.GrenadeAsset>(entity, pos).init(angle);
        if (type == WeaponType.SHOTGUN)
        {
            for (int n = 0; n < 5; ++n)
            {
                Bullet.spawn<BulletLogic.Asset.ShotgunAsset>(entity, pos).init(angle + UnityEngine.Random.Range(-Mathf.PI / 16.0f, Mathf.PI / 16.0f));
            }
        }
        if (type == WeaponType.SNIPER) Bullet.spawn<BulletLogic.Asset.SniperAsset>(entity, pos).init(angle);
        if (type == WeaponType.LASER) Bullet.spawn<BulletLogic.Asset.LaserAsset>(entity, pos).init(angle);
        if (type == WeaponType.XRAY) Bullet.spawn<BulletLogic.Asset.XRayAsset>(entity, pos).init(angle);
    }
}
