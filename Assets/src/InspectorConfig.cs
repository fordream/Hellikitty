using UnityEngine;
using System.Collections;

public class InspectorConfig : MonoBehaviour
{
    public float grid_point_sep = 1.0f;
    public float grid_last_point_sep = 1.0f;

    public float grid_collider_offset = 1.0f;

    public bool debug_verbose_log = true;

    public static InspectorConfig instance = null;

    void Start() { instance = this; }

    public static InspectorConfig get() { return instance; }
}
