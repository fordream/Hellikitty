using System;
using UnityEngine;

public class BasicBullet : Bullet
{
    Bullet parent;
    float angle;
    const float speed = 10.0f;
    GameObject bullet_obj;

    public LayerMask colliders;

    public void init(float angle, int decay_in_ms = 4000)
    {
        add_logic<BulletLogicDecay>().init(decay_in_ms);

        parent = GetComponent<Bullet>();
        this.angle = angle;
        transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += Mathf.Cos(angle) * speed * Time.deltaTime;
        pos.y += Mathf.Sin(angle) * speed * Time.deltaTime;
        transform.position = pos;

        transform.Rotate(new Vector3(0, 0, 2));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & colliders.value) != 0)
        {
            destroy_all();
        }
    }
}
