using System;
using UnityEngine;

namespace BulletLogic.Asset
{
    public class PistolAsset : BulletLogicBase
    {
        private float angle;
        private float damage;
        private float speed;
        private LayerMask colliders;

        public void init(float angle, float damage, float speed, LayerMask colliders, int decay_in_ms = 4000)
        {
            add_logic<BulletLogicDecay>().init(decay_in_ms);

            this.angle = angle;
            this.damage = damage;
            this.speed = speed;
            this.colliders = colliders;
            transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
        }

        private void Update()
        {
            Vector3 pos = transform.position;
            pos.x += Mathf.Cos(angle) * speed * Time.deltaTime;
            pos.y += Mathf.Sin(angle) * speed * Time.deltaTime;
            transform.position = pos;

            transform.Rotate(new Vector3(0, 0, 2));
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (CollidingLayers.is_layer_in_mask(col.gameObject.layer, colliders))
            {
                GenericHealth health = col.gameObject.GetComponent<GenericHealth>();
                if (health != null) health.take_damage(damage);

                destroy_all();
            }
        }
    }
};
