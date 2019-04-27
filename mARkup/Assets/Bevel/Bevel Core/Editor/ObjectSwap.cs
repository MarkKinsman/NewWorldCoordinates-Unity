using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectSwap : ScriptableWizard {

    [Tooltip("Drag in all gameObjects to be replaced with a particular prefab.")]
    public GameObject[] objectsToSwap;
    [Tooltip("Prefab to replace the listed scene objects.")]
    public GameObject replacementPrefab;

    //create the menu and wizard
    [MenuItem("Bevel/Object Swap")]
    static void MakeWizard()
    {
        var swapObjects = DisplayWizard<ObjectSwap>("Swap GameObjects", "Swap");
        swapObjects.objectsToSwap = Selection.gameObjects;
    }

    private void OnWizardCreate()
    {
        //iterate through the list, swapping objects
        for (int i = 0; i < objectsToSwap.Length; i++)
        {
            //create and place the new prefabs
            GameObject replacement = (GameObject)PrefabUtility.InstantiatePrefab(replacementPrefab);
                replacement.transform.position = objectsToSwap[i].transform.position;
                replacement.transform.rotation = objectsToSwap[i].transform.rotation;
                replacement.transform.SetParent(objectsToSwap[i].transform.parent);
            Undo.RegisterCreatedObjectUndo(replacement, "Created replacement " + i.ToString());

            //delete the old objects
            Undo.DestroyObjectImmediate(objectsToSwap[i]);
            DestroyImmediate(objectsToSwap[i]);
        }
    }
}
