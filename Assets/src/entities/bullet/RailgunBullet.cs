using System;
using UnityEngine;

public class RailgunBullet : Bullet
{
    private Bullet parent;
    private float angle;
    private const float MAX_DISTANCE = 10.0f;

    public float damage;
    public float particle_seperation = .5f;

    public void init(float angle)
    {
        init_base();

        parent = GetComponent<Bullet>();
        this.angle = angle;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
                                             MAX_DISTANCE, colliders);
        float dist = hit.transform == null ? MAX_DISTANCE : hit.distance;
        float num_particles = dist / particle_seperation;

        Vector3 pos = transform.position;
        pos.z = -20;
        GameObject railgun_particle = (GameObject)Resources.Load("particles/railgun_lightning");

        for (int n = 0; n < num_particles; ++n)
        {
            pos.x += Mathf.Cos(angle) * particle_seperation;
            pos.y += Mathf.Sin(angle) * particle_seperation;
            GameObject obj = (GameObject)Instantiate(railgun_particle, pos, Quaternion.identity);

            //check if any colliders are overlapping with a circle raycast
            Collider2D col = Physics2D.OverlapCircle(pos, particle_seperation, colliders);
            if (col)
            {
                //if the gameobject has a health component, deal damage to them
                GenericHealth health = col.gameObject.GetComponent<GenericHealth>();
                if (health != null) health.take_damage(damage);
            }
        }
    }
}
