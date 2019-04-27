using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bevel;

namespace Bevel
{
    [CustomEditor(typeof(SpatialButton))]
    public class SpatialButtonEditor : Editor {

        public override void OnInspectorGUI()
        {
            SpatialButton myScript = (SpatialButton)target;

            if (GUILayout.Button("Simulate Click"))
            {
                myScript.calledOnClick.Invoke();
            }

            DrawDefaultInspector();
        }
    }
}

