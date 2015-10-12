using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(RemovalScheduler))]
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public RemovalScheduler removal;

    void Awake()
    {
        removal = GetComponent<RemovalScheduler>();
    }
}
