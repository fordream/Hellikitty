using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 1;
    public Color color;

    float lifetime = 0;

    void Start ()
    {
        transform.GetComponent<Renderer>().material.color = color;
    }
    void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        // Hit?

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.8f, transform.forward);

        for (int i = 0; i < hits.Length; i++)
        {
            // terrain hit
            if (hits[i].collider.gameObject.tag == "Environment")
            {
                // get rid of that m8
                Destroy(this.gameObject);
            }
            // character hit
            if (hits[i].collider.gameObject.tag == "Actor")
            {
                // destroy their health m8
                Debug.Log("Player death ain't in the game yet");
            }
        }
        // No hit ?
        lifetime += Time.deltaTime;

        if (lifetime > 4.0f)
        {
            Destroy(this.gameObject);
        }
	}
}
