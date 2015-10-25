using System;
using UnityEngine;

public class GrenadeBullet : Bullet
{
    private Bullet parent;
    private float angle;
    private const float speed = 10.0f;
    private GameObject bullet_obj;

    public float damage;

    private int explode_after_ms;
    private float explode_timer = 0;

    public void init(float angle, int explode_after_ms = 4000)
    {
        init_base();
        this.explode_after_ms = explode_after_ms;

        parent = GetComponent<Bullet>();
        this.angle = angle;
        transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));

        GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(angle) * 20.0f, Mathf.Sin(angle) * 20.0f), ForceMode2D.Impulse);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 2));

        explode_timer += Time.deltaTime;
        if (explode_timer >= explode_after_ms / 1000.0f)
        {
            destroy_all();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (CollidingLayers.is_layer_in_mask(col.gameObject.layer, colliders))
        {
            GenericHealth health = col.gameObject.GetComponent<GenericHealth>();
            if (health != null)
            {
                health.take_damage(damage);
                destroy_all();
            }
        }
    }
}
