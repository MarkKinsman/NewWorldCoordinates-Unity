using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bevel;

[CustomEditor(typeof(ARMarker))]
public class ARMarkerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        ARMarker myScript = (ARMarker)target;

        if (GUILayout.Button("Simulate Marker Found"))
        {
            myScript.AddAnchor(Vector3.zero, Quaternion.identity);
        }
        if (GUILayout.Button("Simulate Marker Update"))
        {
            myScript.UpdateAnchor(new Vector3(0, -.5f, 1.5f), Quaternion.identity);
        }
        if (GUILayout.Button("Simulate Marker Lost"))
        {
            myScript.RemoveAnchor();
        }

        DrawDefaultInspector();


    }
}
