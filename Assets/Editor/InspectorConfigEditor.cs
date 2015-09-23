using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InspectorConfig))]
public class InspectorConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InspectorConfig.grid_collider_offset = EditorGUILayout.IntField("hey!", (int)InspectorConfig.grid_collider_offset);
        EditorGUILayout.LabelField("test", InspectorConfig.grid_collider_offset.ToString());
    }
}