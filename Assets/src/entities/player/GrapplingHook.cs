using UnityEngine;
using System.Collections.Generic;

public class GrapplingHook : MonoBehaviour {

    Player parent;

    public LayerMask colliders;
    public float max_grapple_dist = 25.0f;
    public float grapple_speed = 30.0f;

    [HideInInspector] public bool grappling = false;
    [HideInInspector] public float grapple_angle = 0;

    List<GameObject> chains = new List<GameObject>();

    public void init()
    {
        parent = GetComponent<Player>();
    }

    public void update() {
        if (grappling)
        {
            Vector3 pos = transform.position;
            pos.x += Mathf.Cos(grapple_angle) * grapple_speed * Time.deltaTime;
            pos.y += Mathf.Sin(grapple_angle) * grapple_speed * Time.deltaTime;
            transform.position = pos;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(grapple_angle), Mathf.Sin(grapple_angle)),
                                                max_grapple_dist, colliders);

            if (hit)
            {
                if (hit.distance <= 1.0f)
                {
                    grappling = false;
                }
                else
                {
                    for (int n = 0; n < chains.Count; ++n)
                    {
                        GameObject chain = chains[n];

                        float dist = Mathf.Sqrt(Mathf.Pow(transform.position.x - chain.transform.position.x, 2) +
                            Mathf.Pow(transform.position.y - chain.transform.position.y, 2));
                        if (dist <= 1.0f)
                        {
                            Destroy(chain);
                            chains.RemoveAt(n);
                            --n;
                        }
                    }
                }
            }else grappling = false;

            if (!grappling) {
                foreach (GameObject chain in chains)
                {
                    Destroy(chain);
                }
                chains.Clear();
            }
        }else if (Input.GetMouseButtonUp(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(parent.gun.angle), Mathf.Sin(parent.gun.angle)),
                                                 max_grapple_dist, colliders);
            if (hit && hit.distance >= 4.0f)
            {
                grappling = true;
                grapple_angle = parent.gun.angle;

                float sep = .7f;
                float num_particles = hit.distance / sep;

                Vector3 pos = transform.position;
                pos.z = -20;
                GameObject railgun_particle = (GameObject)Resources.Load("bullets/chain");

                for (int n = 0; n < num_particles; ++n)
                {
                    pos.x += Mathf.Cos(grapple_angle) * sep;
                    pos.y += Mathf.Sin(grapple_angle) * sep;
                    GameObject obj = (GameObject)Instantiate(railgun_particle, pos, Quaternion.identity);
                    obj.transform.localEulerAngles = new Vector3(0, 0, grapple_angle * (180.0f / Mathf.PI) + 90);
                    chains.Add(obj);
                }
            }
        }
	}
}
