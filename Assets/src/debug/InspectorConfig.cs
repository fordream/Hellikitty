﻿using UnityEngine;
using System.Collections;

public class InspectorConfig : MonoBehaviour
{
    /* grid config */
    public bool grid_debug_display = false;
    public bool grid_last_debug_display = false;

    public float grid_point_sep = 1.0f;
    public float grid_last_point_sep = 1.0f;

    public float grid_terrain_offset = .5f;

    public LayerMask grid_collidable_layers;

    /* debug config */
    public bool debug_verbose_log = true;
}