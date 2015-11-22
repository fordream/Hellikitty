using System;
using UnityEngine;

public class WeaponPistol : Weapon
{
    public float damage = 10.0f;
    public float speed = 30.0f;
    public LayerMask colliders;

    private void Awake()
    {
        weapon_asset = (GameObject)Resources.Load("weapons/pistol");
    }

    private void Update()
    {
        if (fire_ready) fire();
    }

    public void fire()
    {
        Bullet.spawn<BulletLogic.Asset.PistolAsset>(entity, pos).init(angle, damage, speed, colliders);
    }
}
