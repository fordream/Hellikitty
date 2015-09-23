using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InspectorConfig))]
public class InspectorConfigEditor : Editor
{
    bool show_debug_config = true;
    bool show_grid_config = true;

    public override void OnInspectorGUI()
    {
        draw_debug_gui();
        draw_grid_gui();
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
                InspectorConfig.debug_verbose_log = EditorGUILayout.Toggle(new GUIContent("Verbose logging", 
                    "If enabled, calls to Debug.LogVerbose will activate (more detailed info messages)"),
                InspectorConfig.debug_verbose_log);
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
                InspectorConfig.grid_point_seperation = EditorGUILayout.FloatField(new GUIContent("Point seperation", 
                    "Determines how far apart each point is from each other on the grid " +
                    "(in world space units). The smaller the value, the more nodes there are on the pathfinding grid"), 
                    InspectorConfig.grid_point_seperation);

                InspectorConfig.grid_collider_offset = EditorGUILayout.FloatField(new GUIContent("Size offset", 
                    "When checking whether an object is collidable or not on the grid, this value determines how much " + 
                    "larger the area around the terrain is (in world space units)"), 
                    InspectorConfig.grid_collider_offset);
            });
        }
    }
}