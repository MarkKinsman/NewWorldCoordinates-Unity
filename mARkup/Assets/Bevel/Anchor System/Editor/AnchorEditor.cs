using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bevel;

[CustomEditor(typeof(Anchor))]
public class AnchorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Anchor myScript = (Anchor)target;
        if (GUILayout.Button("Simulate Anchor Found"))
        {
            myScript.TargetFoundHandler(true);
        }
        if (GUILayout.Button("Simulate Anchor Update"))
        {
            myScript.UpdateTargetSimulator();
        }

        if (GUILayout.Button("Simulate Anchor Lost"))
        {
            myScript.OnLoseTarget();
        }

        DrawDefaultInspector();

        if (GUILayout.Button("Prebake Anchor Position"))
        {
            myScript.PrebakePlacement();
        }

    }
}
