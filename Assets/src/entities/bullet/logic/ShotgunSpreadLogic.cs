using System;
using UnityEngine;

namespace BulletLogic
{
    public class Shotgunlogic : BulletLogicBase
    {
        public void init(float angle, int decay_in_ms = 4000)
        {
            Bullet.spawn<Asset.ShotgunAsset>(entity, transform.position).init(angle, decay_in_ms);
            Destroy(this);
        }
    }
};