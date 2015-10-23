using System;
using UnityEngine;

public class BulletLogicBase : MonoBehaviour
{
    public void destroy_all()
    {
        if (transform.gameObject) Destroy(transform.gameObject);
    }
}
