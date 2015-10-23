using System;
using UnityEngine;

public class LogicBase : MonoBehaviour
{
    public void destroy_all()
    {
        if (transform.gameObject) Destroy(transform.gameObject);
    }
}
