using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class InvertMesh : ScriptableWizard
{

    [Tooltip("Drag in all gameObjects to be replaced with a particular prefab.")]
    public GameObject[] objectsToInvert;

    //create the menu and wizard
    [MenuItem("Bevel/Invert Mesh")]
    static void MakeWizard()
    {
        var Inverting = DisplayWizard<InvertMesh>("Invert Mesh", "Invert");
        Inverting.objectsToInvert = Selection.gameObjects;
    }

    private void OnWizardCreate()
    {
        //iterate through the list, swapping objects
        for (int i = 0; i < objectsToInvert.Length; i++)
        {
            //create mesh variable, invert it and assign it to mesh
            Mesh mesh = objectsToInvert[i].GetComponent<MeshFilter>().sharedMesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
            Undo.RegisterCompleteObjectUndo(objectsToInvert[i], "Inverted Object - " + objectsToInvert[i].name);

        }
    }
}
