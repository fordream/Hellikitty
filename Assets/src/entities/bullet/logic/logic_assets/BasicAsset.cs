using System;
using UnityEngine;

namespace BulletLogic.Asset
{
    public class BasicAsset : BulletLogicBase
    {
        private float angle;

        public float damage;
        public float speed = 10.0f;

        public LayerMask player_owner_collide_layers;
        public LayerMask enemy_owner_collide_layers;
        [HideInInspector] public LayerMask colliders;

        public void init(float angle, int decay_in_ms = 4000)
        {
            colliders = entity == Entities.player ? player_owner_collide_layers : enemy_owner_collide_layers;

            add_logic<BulletLogicDecay>().init(decay_in_ms);

            this.angle = angle;
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
