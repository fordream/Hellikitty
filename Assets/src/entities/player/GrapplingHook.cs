using UnityEngine;
using System.Collections.Generic;

public class GrapplingHook : MonoBehaviour
{

    public enum GrappleState
    {
        NONE, 
        SHOOTING_OUT, 
        FOLLOWING
    };

    Player parent;

    public LayerMask colliders;
    public float grapple_radius = 25.0f;
    public float grapple_speed = 30.0f;
    public float chain_seperation = .7f;
    public float chain_shootout_speed = 400;
    public float chain_break_time = .1f;

    float num_chains;
    Vector3 chain_pos;
    [HideInInspector] public GrappleState grapple_state = GrappleState.NONE;

    [HideInInspector] public float grapple_angle = 0;
    bool shooting_grapple = false;

    List<GameObject> chains = new List<GameObject>();

    float non_collide_timer = 0;

    public void init()
    {
        parent = GetComponent<Player>();
    }

    public void update() {
        RaycastHit2D hit;

        switch (grapple_state)
        {
            case GrappleState.SHOOTING_OUT:
                GameObject chain_sprite = (GameObject)Resources.Load("bullets/chain");

                for (int n = chains.Count; n < num_chains; ++n)
                {
                    chain_pos.x += Mathf.Cos(grapple_angle) * chain_seperation;
                    chain_pos.y += Mathf.Sin(grapple_angle) * chain_seperation;
                    GameObject obj = (GameObject)Instantiate(chain_sprite, chain_pos, Quaternion.identity);
                    obj.transform.localEulerAngles = new Vector3(0, 0, grapple_angle * (180.0f / Mathf.PI) + 90);
                    chains.Add(obj);

                    if (n >= num_chains - 1) grapple_state = GrappleState.FOLLOWING;
                    if (n >= chain_shootout_speed * Time.deltaTime) break;
                }
                break;
            case GrappleState.FOLLOWING:
                Vector3 pos = transform.position;
                pos.x += Mathf.Cos(grapple_angle) * grapple_speed * Time.deltaTime;
                pos.y += Mathf.Sin(grapple_angle) * grapple_speed * Time.deltaTime;
                transform.position = pos;
                
                hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(grapple_angle), Mathf.Sin(grapple_angle)),
                                                     grapple_radius, colliders);

                if (hit)
                {
                    if (hit.distance <= 1.0f) grapple_state = GrappleState.NONE;
                    else
                    {
                        non_collide_timer += Time.deltaTime;
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
                                non_collide_timer = 0;
                            }
                        }
                        if (non_collide_timer >= chain_break_time) grapple_state = GrappleState.NONE;
                    }
                }else grapple_state = GrappleState.NONE;

                //if grapple state is now none, remove all chains from the list
                if (grapple_state == GrappleState.NONE) {
                    foreach (GameObject chain in chains)
                    {
                        Destroy(chain);
                    }
                    chains.Clear();
                }
                break;
            default:
                if (Input.GetMouseButtonUp(1))
                {
                    hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(parent.gun.angle), Mathf.Sin(parent.gun.angle)),
                                                         grapple_radius, colliders);
                    if (hit && hit.distance >= 4.0f)
                    {
                        grapple_angle = parent.gun.angle;
                        grapple_state = GrappleState.SHOOTING_OUT;

                        num_chains = hit.distance / chain_seperation;
                        chain_pos = transform.position;
                        chain_pos.z = -20;
                    }
                }
                break;
        }
	}
}
