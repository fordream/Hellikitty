using System;
using UnityEngine;

public class RailgunBullet : MonoBehaviour
{
    Bullet parent;
    float angle;
    const float MAX_DISTANCE = 10.0f;

    public void init(float _angle)
    {
        parent = GetComponent<Bullet>();
        angle = _angle;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
                                             MAX_DISTANCE, 1024 | Debug.config.grid_collidable_layers);
        float dist = hit.transform == null ? MAX_DISTANCE : hit.distance;
        float particle_size = .5f;
        float num_particles = dist / particle_size;

        Vector3 pos = transform.position;
        pos.z = -20;
        GameObject railgun_particle = (GameObject)Resources.Load("particles/railgun_lightning");

        for (int n = 0; n < num_particles; ++n)
        {
            pos.x += Mathf.Cos(angle) * particle_size;
            pos.y += Mathf.Sin(angle) * particle_size;
            GameObject obj = (GameObject)Instantiate(railgun_particle, pos, Quaternion.identity);
        }
    }
}
