using UnityEngine;
using System.Collections;

public class InspectorConfig : MonoBehaviour
{
    /* grid config */
    public bool grid_debug_display = false;
    public bool grid_last_debug_display = false;

    public float grid_point_sep = 1.0f;
    public float grid_last_point_sep = 1.0f;

    public float grid_size_offset = 1.0f;


    /* debug config */
    public bool debug_verbose_log = true;


    //singleton instance
    public static InspectorConfig instance = null;

    void Start() { instance = this; }

    public static InspectorConfig get() { return instance; }
}
