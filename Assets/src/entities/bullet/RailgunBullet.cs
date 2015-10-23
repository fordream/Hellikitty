using System;
using UnityEngine;

public class RailgunBullet : Bullet
{
    Bullet parent;
    float angle;
    const float MAX_DISTANCE = 10.0f;

    public LayerMask colliders;
    public float particle_seperation = .5f;

    public void init(float angle)
    {
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
        }
    }
}
