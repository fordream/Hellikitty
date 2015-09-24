using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InspectorConfig))]
public class InspectorConfigEditor : Editor
{
    bool show_debug_config = true;
    bool show_grid_config = true;
    InspectorConfig config;

    void OnEnable()
    {
        config = (InspectorConfig)target;
    }

    public override void OnInspectorGUI()
    {
        draw_debug_gui();
        draw_grid_gui();

        if (GUI.changed) EditorUtility.SetDirty(target);
    }

    void draw_debug_gui()
    {
        EditorGUI.indentLevel = 0;
        show_debug_config = EditorGUILayout.Foldout(show_debug_config, "Debug Config");

        if (show_debug_config)
        {
            EditorGUI.indentLevel = 2;
            Ferr_EditorTools.Box(4, () =>
            {
                config.debug_verbose_log = EditorGUILayout.Toggle(new GUIContent("Verbose logging", 
                    "If enabled, calls to Debug.LogVerbose will activate (more detailed info messages)"),
                config.debug_verbose_log);
            });
        }
    }

    void draw_grid_gui()
    {
        EditorGUI.indentLevel = 0;
        show_grid_config = EditorGUILayout.Foldout(show_grid_config, "Pathfinding Grid");

        if (show_grid_config)
        {
            EditorGUI.indentLevel = 2;
            Ferr_EditorTools.Box(4, () =>
            {
                config.grid_debug_display = EditorGUILayout.Toggle(new GUIContent("Debug display",
                    "Displays and updates waypoint grid every frame and shows all terrain collider size offsets"),
                config.grid_debug_display);
                if (Application.isPlaying)
                {
                    if (config.grid_last_debug_display != config.grid_debug_display && config.grid_debug_display)
                    {
                        WaypointGrid.get().recreate_grid();
                    }
                }
                config.grid_last_debug_display = config.grid_debug_display;

                config.grid_point_sep = EditorGUILayout.FloatField(new GUIContent("Point seperation",
                    "Determines how far apart each point is from each other on the grid " +
                    "(in world space units). The smaller the value, the more nodes there are on the pathfinding grid"),
                    config.grid_point_sep);
                config.grid_point_sep = Mathf.Clamp(config.grid_point_sep, .25f, 5.0f);
                if (Application.isPlaying)
                {
                    if (config.grid_last_point_sep != config.grid_point_sep) WaypointGrid.instance.recreate_grid();
                }
                config.grid_last_point_sep = config.grid_point_sep;

                config.grid_size_offset = EditorGUILayout.FloatField(new GUIContent("Size offset",
                    "When checking whether an object is collidable or not on the grid, this value determines how much " +
                    "larger the area around the terrain is (in world space units)"),
                    config.grid_size_offset);
            });
        }
    }
}