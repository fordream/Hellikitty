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
        transform.localEulerAngles = new Vector3(0, 0, angle * (180.0f / Mathf.PI));
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "obstacle")
        {
            Destroy(gameObject);
        }
    }
}
