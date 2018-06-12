using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetCreator))]
public class TargetCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TargetCreator targetCreator = (TargetCreator)target;
        if (GUILayout.Button("Create targets"))
        {
            targetCreator.CreateTargets();
        }
    }
}
