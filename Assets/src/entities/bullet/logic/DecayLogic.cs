using System;
using UnityEngine;

public class DecayLogic : LogicBase
{
    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
    int decay_in_ms;

    public void init(int decay_in_ms)
    {
        this.decay_in_ms = decay_in_ms;
        watch.Start();
    }

    void Update()
    {
        if (watch.ElapsedTicks / 10000.0f >= decay_in_ms)
        {
            destroy_all();
        }
    }
}
