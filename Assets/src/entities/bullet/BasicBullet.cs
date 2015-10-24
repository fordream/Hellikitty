using System;
using UnityEngine;

public class BasicBullet : Bullet
{
    private Bullet parent;
    private float angle;
    private const float speed = 10.0f;
    private GameObject bullet_obj;
    private const float DAMAGE = 1.0f;

    public LayerMask colliders;

    public void init(float angle, int decay_in_ms = 4000)
    {
        add_logic<BulletLogicDecay>().init(decay_in_ms);

        parent = GetComponent<Bullet>();
        this.angle = angle;
        transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x += Mathf.Cos(angle) * speed * Time.deltaTime;
        pos.y += Mathf.Sin(angle) * speed * Time.deltaTime;
        transform.position = pos;

        transform.Rotate(new Vector3(0, 0, 2));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (CollidingLayers.is_layer_in_mask(col.gameObject.layer, colliders.value))
        {
            GenericHealth health = col.gameObject.GetComponent<GenericHealth>();
            if (health != null) health.take_damage(DAMAGE);

            destroy_all();
        }
    }
}
