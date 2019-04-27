using System;
using System.Collections;
using System.Collections.Generic;
using Bevel;
using UnityEngine;

public class ObjectSelection : MonoBehaviour {

    public Material hilightMaterial;
    public LayerMask selectableLayers;
    public bool isCropped;
    public bool isSingleSelectOnly;

    //private
    public List<GameObject> selectedObjects; //TODO: I add to and remove stuff from this list probably hilight like I did in the paint one.
    public List<GameObject> hilights;

	// Use this for initialization
	void Start ()
    {
        //setup subscription to clicks based on exposed options
        if (isSingleSelectOnly)
        {
            if (isCropped)
            {
                BevelInput.clickObjectEventCropped += SelectSingleObject;
            }
            else
            {
                BevelInput.clickObjectEvent += SelectSingleObject;
            }

        }
        else
        {
            if (isCropped)
            {
                BevelInput.clickObjectEventCropped += Select;
            }
            else
            {
                BevelInput.clickObjectEvent += Select;
            }

        }

    }
	
    //for selecting multiple objects one at a time. 
    public void Select(GameObject thing)
    {
        if (selectableLayers == (selectableLayers | (1 << thing.layer)))
        {
            if (!selectedObjects.Contains(thing))
            {
                selectedObjects.Add(thing);
                HilightLight(thing);
            }
            else
            {
                Deselect(thing);
            }
        }
    }

    //for selecting multiple objects at a time.
    //Honestly not sure when this will be used but it seemed right to make  the option.
    public void Select(GameObject[] things)
    {
        foreach (var item in things)
        {
            Select(item);
        }
    }

    //only allows for a single object to be selected at once.
    //When an object is selected, it empties the list and starts from scratch. 
    public void SelectSingleObject(GameObject clickedThing)
    {
        //checks first if item is already selected. if it is, deselects.
        foreach (var item in selectedObjects)
        {
            if (clickedThing == item)
            {
                DeselectAll();
                return;
            }
        }
        DeselectAll();
        Select(clickedThing);
    }

    //dehilights everything and empties the list. Start over from zero. Default state.
    public void DeselectAll()
    {
        if (selectedObjects.Count > 0)
        {
            foreach (var item in selectedObjects)
            {
                DeHilightLight(item);
            }
        }

        selectedObjects = new List<GameObject>();
    }

    //single deselct.
    public void Deselect(GameObject thing)
    {
        try
        {
            DeHilightLight(thing);
        }
        catch (Exception e)
        {
            Debug.LogError("failed to dehilight");
            Console.WriteLine(e);
            throw;
        }

        try
        {
            selectedObjects.Remove(thing);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to remove selected object");
            Console.WriteLine(e);
            throw;
        }
        
    }

    //changes the layer of the object so a layer-controlled light shines on it.
    public void HilightLight(GameObject thing){
        Transform[] children = thing.GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            thing.layer = 16;
        }

    }
    //changes layer back to content so the layer-controlled light no longer shines on it.
    public void DeHilightLight(GameObject thing){
        Transform[] children = thing.GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            item.gameObject.layer = 15;
        }

    }


    //Fuck, I should just delete this. 
    #region Old Hilight system. Keeping only for reference. It didn't work well. 
    //makes a big box for hilighting. weird and ugly.
    public void Hilight(GameObject thing) //TODO: get meshrender components in children and hilight all of them. Incase there are nested objects with their own shit.
    {

        GameObject hilight = GameObject.CreatePrimitive(PrimitiveType.Cube);



        hilight.name = "Hilight";
        hilight.transform.parent = thing.transform;
        hilight.transform.position = thing.GetComponent<MeshRenderer>().bounds.center;
        hilight.transform.localRotation = thing.transform.localRotation;
        hilight.transform.localScale = new Vector3(
            thing.GetComponent<MeshRenderer>().bounds.size.x * thing.transform.localScale.x / thing.transform.lossyScale.x,
            thing.GetComponent<MeshRenderer>().bounds.size.y * thing.transform.localScale.y / thing.transform.lossyScale.y,
            thing.GetComponent<MeshRenderer>().bounds.size.z * thing.transform.localScale.z / thing.transform.lossyScale.z);
        hilight.GetComponent<MeshRenderer>().material = hilightMaterial;
        hilight.SetActive(true);

        hilights.Add(hilight);
    }

    public void DeHilight(GameObject thing)
    {

        GameObject hilightToGo = hilights.ToArray()[selectedObjects.IndexOf(thing)];
        hilights.Remove(hilightToGo);
        Destroy(hilightToGo);
    }
    #endregion
}
