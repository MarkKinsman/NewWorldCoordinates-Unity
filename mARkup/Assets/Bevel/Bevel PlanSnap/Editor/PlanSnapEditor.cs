using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanSnap))]
public class PlanSnapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlanSnap myScript = (PlanSnap)target;
        if (GUILayout.Button("Save Transforms"))
        {
            myScript.SaveTransforms();
        }
        if (GUILayout.Button("Load Transforms"))
        {
            myScript.StartAR();
        }

        DrawDefaultInspector();

        AddSpatialButton(myScript);

    }

    void AddSpatialButton(PlanSnap script)
    {
        if (!script.gameObject.GetComponent<SpatialButton>())
        {
            SpatialButton button = script.gameObject.AddComponent<SpatialButton>();
            button.calledOnClick.AddListener(script.StartAR);
        }
    }
}
