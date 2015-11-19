using UnityEngine;
using System.Collections;

public class ColliderActions : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();//lachlan added playing of audio
            transform.position = new Vector2(-10, -10);
            Destroy(coll.gameObject);
        }
    }
}