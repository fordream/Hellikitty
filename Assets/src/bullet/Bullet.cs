﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(RemovalScheduler))]
public class Bullet : MonoBehaviour
{
    public RemovalScheduler removal;

    void Start()
    {
        removal = GetComponent<RemovalScheduler>();
    }
}
