using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HierarchyCleanUp : ScriptableWizard {

    [Tooltip("Drag in parent gameObject[s] with messy hierarchy.")]
    public GameObject[] objectsToClean;

    int objectsDeleted = 0;

    //create the menu and wizard
    [MenuItem("Bevel/Hierarchy Cleanup")]
    static void MakeWizard()
    {
        var cleanObject = DisplayWizard<HierarchyCleanUp>("Clean Hierarchies", "Clean");
        cleanObject.objectsToClean = Selection.gameObjects;
    }

    private void OnWizardCreate()
    {
        //iterate through the selection set, swapping objects
        for (int i = 0; i < objectsToClean.Length; i++)
        {
            //after testing this round, add this multiple runs through
            int cleanedSoFar = 0;
            //do
            //{

            //} while (objectsDeleted != cleanedSoFar);
            cleanedSoFar += CleanupIteration(objectsToClean[i]);
            objectsDeleted += cleanedSoFar;
        }
        Debug.Log("Cleaned up Objects: " + objectsDeleted);
    }

    int CleanupIteration(GameObject objectToClean)
    {
        Debug.Log("Cleaning " + objectToClean.name);
        int cleanedThisRound = 0;
        //iterate through children of a selected object. 
        Transform[] objectChildren = objectToClean.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < objectChildren.Length; i++)
        {
            Transform thingInQuestion = objectChildren[i];

            //if it is empty then move stuff out and get rid of it
            if (thingInQuestion.gameObject.GetComponent<MeshRenderer>() == null)
            {
                Transform[] thingsChildren = thingInQuestion.GetComponentsInChildren<Transform>();
                //check for children and move them up in the hierarchy
                if (thingsChildren.Length > 1)
                {
                    for (int j = 1; j < thingsChildren.Length; j++)
                    {
                        thingsChildren[j].SetParent(thingInQuestion.parent);
                    }

                }
                //delete and step the counter
                GameObject.DestroyImmediate(objectChildren[i].gameObject);
                cleanedThisRound++;
            }

        }
        return cleanedThisRound;
    }

}
