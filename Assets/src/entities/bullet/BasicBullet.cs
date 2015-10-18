using System;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    Bullet parent;
    float angle;
    const float speed = 10.0f;
    GameObject bullet_obj;

    public void init(float _angle)
    {
        parent = GetComponent<Bullet>();
        angle = _angle;
        transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));

        bullet_obj = GameObject.Instantiate((GameObject)Resources.Load("bullets/triangle_bullet"));
        bullet_obj.transform.parent = parent.transform;
        bullet_obj.transform.localPosition = new Vector3(0, 0, transform.position.z);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += Mathf.Cos(angle) * speed * Time.deltaTime;
        pos.y += Mathf.Sin(angle) * speed * Time.deltaTime;
        transform.position = pos;

        transform.Rotate(new Vector3(0, 0, 2));

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "obstacle")
        {
            Destroy(gameObject);
        }
    }
}
