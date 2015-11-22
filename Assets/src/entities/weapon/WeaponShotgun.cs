using System;
using UnityEngine;

public class WeaponShotgun : Weapon
{
    public float damage = 10.0f;
    public float speed = 30.0f;
    public int num_bullets_shot = 5;
    public float bullet_spread_degrees = 10.0f;
    public LayerMask colliders;

    private void Awake()
    {
        weapon_asset = (GameObject)Resources.Load("weapons/shotgun");
    }

    private void Update()
    {
        if (fire_ready) fire();
    }

    public void fire()
    {
        float bullet_spread_radians = bullet_spread_degrees / (180 / Mathf.PI);
        for (int n = 0; n < num_bullets_shot; ++n)
        {
            float spread_angle = angle + UnityEngine.Random.Range(-bullet_spread_radians, bullet_spread_radians);
            Bullet.spawn<BulletLogic.Asset.ShotgunAsset>(entity, pos).init(spread_angle, damage, speed, colliders);
        }
    }
}
