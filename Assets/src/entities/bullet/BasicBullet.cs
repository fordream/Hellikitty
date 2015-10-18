using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BasicBullet : MonoBehaviour
{
    Bullet parent;
    float angle;
    const float speed = 10.0f;

    public void init(float _angle)
    {
        parent = GetComponent<Bullet>();
        angle = _angle;
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
        if (col.gameObject.tag == "obstacle")
        {
            Destroy(gameObject);
        }
    }
}
