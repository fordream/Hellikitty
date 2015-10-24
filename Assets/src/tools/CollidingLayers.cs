using System;
using UnityEngine;

/*
* A simple helper class to add layers and check if they contain
* other layers.
* For example, when colliding with something, you can add the collision
* layer number to this object and then later you can check if
* that layer is contained in here.
* Also, remember to reset()!
*/

public class CollidingLayers
{
    private int value;

    public void add(int layer)
    {
        value |= 1 << layer;
    }

    public bool contains(string layerName)
    {
        return contains(LayerMask.NameToLayer(layerName));
    }

    public bool contains(int layer)
    {
        return (value & (1 << layer)) != 0;
    }

    public void reset()
    {
        value = 0;
    }

    public static bool is_layer_in_mask(int layer, LayerMask mask)
    {
        return ((1 << layer) & mask.value) != 0;
    }
}
