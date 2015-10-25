using System;
using UnityEngine;

namespace BulletLogic.Asset
{
    public class GrenadeAsset : BulletLogicBase
    {
        private const float speed = 10.0f;

        public float max_damage = 5.0f;
        public float explosion_radius = 4.0f;

        private int explode_after_ms;
        private float explode_timer = 0;

        public LayerMask explode_layers;

        public void init(float angle, int explode_after_ms = 2500)
        {
            this.explode_after_ms = explode_after_ms;

            transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));

            GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(angle) * speed, 5 + (Mathf.Sin(angle) * speed)), ForceMode2D.Impulse);
        }

        private void Update()
        {
            explode_timer += Time.deltaTime;
            if (explode_timer >= explode_after_ms / 1000.0f)
            {
                explode();
                destroy_all();
            }
        }

        private void explode()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosion_radius, explode_layers);
            foreach (Collider2D col in cols)
            {
                GenericHealth health = col.gameObject.GetComponent<GenericHealth>();
                if (health != null)
                {
                    Instantiate((GameObject)Resources.Load("particles/explosion"),
                                new Vector3(transform.position.x, transform.position.y, -20), Quaternion.identity);

                    float dist = Vector2.Distance(col.transform.position, transform.position);
                    dist = Mathf.Min(dist, explosion_radius);
                    float damage = ((explosion_radius - dist) / explosion_radius) * max_damage;
                    health.take_damage(damage);
                }
            }
        }
    }
};