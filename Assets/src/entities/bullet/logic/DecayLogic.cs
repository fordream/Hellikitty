using System;
using UnityEngine;

namespace BulletLogic
{
    public class BulletLogicDecay : BulletLogicBase
    {
        int decay_in_ms;
        float decay_timer = 0;

        public void init(int decay_in_ms)
        {
            this.decay_in_ms = decay_in_ms;
        }

        void Update()
        {
            decay_timer += Time.deltaTime;
            if (decay_timer >= decay_in_ms / 1000.0f)
            {
                destroy_all();
            }
        }
    }
};