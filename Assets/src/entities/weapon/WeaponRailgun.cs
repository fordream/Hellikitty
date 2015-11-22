using System;
using UnityEngine;

public class WeaponRailgun : Weapon
{
    public float damage = 8.0f;
    public float max_distance = 10.0f;
    public LayerMask colliders;

    private void Awake()
    {
        weapon_asset = (GameObject)Resources.Load("weapons/railgun");
    }

    private void Update()
    {
        if (fire_ready) fire();
    }

    public void fire()
    {
        Bullet.spawn<BulletLogic.Asset.RailgunAsset>(entity, pos).init(angle, damage, max_distance, colliders);
    }
}
