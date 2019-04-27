using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour {

    public Material hilightMaterial;
    public LayerMask selectableLayers;

    //private
    public List<GameObject> selectedObjects; //TODO: I add to and remove stuff from this list probably hilight like Idid in the paint one.
    public List<GameObject> hilights;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Select(GameObject thing)
    {
        if (selectableLayers == (selectableLayers | (1 << thing.layer)))
        {

        }
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

    public void Select(GameObject[] things)
    {
        foreach (var item in things)
        {
            Select(item);
        }
    }

    public void SelectSingleObject(GameObject thing)
    {
        DeselectAll();
        Select(thing);
    }

    public void DeselectAll()
    {
        if (selectedObjects.Count > 0)
        {
            foreach (var item in selectedObjects)
            {
                Deselect(item);
            }
        }
    }

    public void Deselect(GameObject thing)
    {
        DeHilightLight(thing);
        selectedObjects.Remove(thing);
        
    }

    public void HilightLight(GameObject thing){
        Transform[] children = thing.GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            thing.layer = 16;
        }

    }
    public void DeHilightLight(GameObject thing){
        Transform[] children = thing.GetComponentsInChildren<Transform>();
        foreach (var item in children)
        {
            thing.layer = 15;
        }

    }

    public void Hilight(GameObject thing) //TODO: get meshrender components in children and hilight all of them. Incase there are nested objects with their own shit.
    {

        GameObject hilight = GameObject.CreatePrimitive(PrimitiveType.Cube);



        hilight.name = "Hilight";
        hilight.transform.parent = thing.transform;
        hilight.transform.position = thing.GetComponent<MeshRenderer>().bounds.center ;
        hilight.transform.localRotation = thing.transform.localRotation;
        hilight.transform.localScale = new Vector3(
            thing.GetComponent<MeshRenderer>().bounds.size.x * thing.transform.localScale.x/thing.transform.lossyScale.x,
            thing.GetComponent<MeshRenderer>().bounds.size.y * thing.transform.localScale.y / thing.transform.lossyScale.y,
            thing.GetComponent<MeshRenderer>().bounds.size.z * thing.transform.localScale.z / thing.transform.lossyScale.z) ;
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
}
