using System;
using System.Collections;
using System.Collections.Generic;
using Bevel;
using UnityEngine;
using Hyperreal;



public class PlanSnap : MonoBehaviour {

    [SerializeField]
    [Tooltip("Drag in every Category that turns on with this view. All others will be turned off.")]
    Category[] catergoriesOnAtStart;


    [SerializeField]
    private Vector3 objectScale = new Vector3(1, 1, 1);
    [SerializeField]
    private Vector3 objectPosition = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 cropBoxScale = new Vector3(1, 1, 1);
    [SerializeField]
    private Vector3 cropBoxPosition = new Vector3(0,0,0);

    PlanSnapManager planSnapManager;
    GameObject managedObject;
    BoxMaskMaterialAligner cropBox;

    public bool isSnappedOnEnabled;

    private void OnEnable()
    {
        if (isSnappedOnEnabled)
        {
            StartAR();
        }
    }

    // Use this for initialization
    void Start () {
        PopulateManager();

        //add listener for to the spatial button (only works at runtime)
        if (GetComponent<SpatialButton>())
        {
            GetComponent<SpatialButton>().calledOnClick.AddListener(StartAR);
        }
    }

    //try to populate plan snap manager
    void PopulateManager()
    {
        if (!planSnapManager)
        {
            try
            {
                planSnapManager = FindObjectOfType<PlanSnapManager>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        if (!managedObject)
        {
            try
            {
                managedObject = planSnapManager.managedObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        if (!cropBox)
        {
            try
            {
                cropBox = planSnapManager.cropBox;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }

    //For use in the editor to easily and visually save transform info of the managed object into the plansnap
    public void SaveTransforms()
    {
        //populate the object manager fields even if in editor
        PopulateManager();

        //Attempt to save the locations to the plansnap object
        if (managedObject)
        {
            objectScale = managedObject.transform.localScale;
            objectPosition = managedObject.transform.localPosition;
        }
        else
        {
            Debug.LogWarning("No managed object found. Check plan snap manager.");
        }
        if (cropBox)
        {
            cropBoxScale = cropBox.transform.localScale;
            cropBoxPosition = cropBox.transform.localPosition;
        }
        else
        {
            Debug.LogWarning("No crop box found. If you intended to use cropbox, check plan snap manager.");
        }

    }

    public void StartAR()
    {
        //populate the object manager fields even if in editor
        PopulateManager();

        //turn off all the other AR Buttons
        foreach (var item in planSnapManager.planSnaps)
        {
            item.StopAR();
        }

        //apply saved position and scale to the objects
        if (managedObject)
        {
            managedObject.transform.localScale = objectScale;
            managedObject.transform.localPosition = objectPosition;
        }
        if (cropBox)
        {
            cropBox.transform.localPosition = cropBoxPosition;
            cropBox.transform.localScale = cropBoxScale;
            if (cropBox.GetComponent<BoxCull>() && Application.isPlaying)
            {
                cropBox.GetComponent<BoxCull>().CullOutsideBox();
            }
            else
            {
                Debug.LogWarning("No box cull script found. " +
                    "Performance would be improved if invisible geometry were culled from the scene.");
            }
        }

        SetVisibleCategories(catergoriesOnAtStart);

        //make the AR button disappear
        GetComponentInChildren<MeshRenderer>(true).enabled = false;
        GetComponentInChildren<BoxCollider>(true).enabled = false;
    }

    //Turn off all categories then turn on the selected categories
    private void SetVisibleCategories(Category[] categories)
    {
        try
        {
            Category[] allCategories = FindObjectsOfType<Category>();
            foreach (var item in allCategories)
            {
                item.TurnOff();
            }

            foreach (var item in catergoriesOnAtStart)
            {
                item.TurnOn();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("No categories found (I think). Ignoring layer control");
            throw e;
        }
    }

    public void StopAR()
    {
        GetComponentInChildren<MeshRenderer>(true).enabled = true;
        GetComponentInChildren<BoxCollider>(true).enabled = true;
    }
	
}
