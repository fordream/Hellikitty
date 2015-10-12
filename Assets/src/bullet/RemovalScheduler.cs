using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RemovalScheduler : MonoBehaviour
{
    bool scheduled = false;

    public bool is_scheduled() { return scheduled; }
    public void schedule() { scheduled = true; }
}
