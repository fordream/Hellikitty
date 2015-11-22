using System;
using UnityEngine;

namespace BulletLogic.Asset
{
    public class RailgunAsset : BulletLogicBase
    {
        private float angle;
        private float max_distance;
        private float damage;
        private LayerMask colliders;

        private const float PARTICLE_SEP = .5f;

        public void init(float angle, float damage, float max_distance, LayerMask colliders)
        {
            this.angle = angle;
            this.damage = damage;
            this.max_distance = max_distance;
            this.colliders = colliders;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
                                                 max_distance, colliders);
            float dist = hit.transform == null ? max_distance : hit.distance;
            float num_particles = dist / PARTICLE_SEP;

            Vector3 pos = transform.position;
            pos.z = -20;
            GameObject railgun_particle = (GameObject)Resources.Load("particles/railgun_lightning");

            for (int n = 0; n < num_particles; ++n)
            {
                pos.x += Mathf.Cos(angle) * PARTICLE_SEP;
                pos.y += Mathf.Sin(angle) * PARTICLE_SEP;
                GameObject obj = (GameObject)Instantiate(railgun_particle, pos, Quaternion.identity);

                //check if any colliders are overlapping with a circle raycast
                Collider2D col = Physics2D.OverlapCircle(pos, PARTICLE_SEP, colliders);
                if (col)
                {
                    //if the gameobject has a health component, deal damage to them
                    GenericHealth health = col.gameObject.GetComponent<GenericHealth>();
                    if (health != null) health.take_damage(damage);
                }
            }
        }
    }
};