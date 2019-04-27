using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.XR.iOS;
#endif
using Hyperreal;
using Bevel;


namespace Bevel
{
    public class PlanSnapManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Will autofill with ObjectManager. Drag in object to use in editor")]
        public GameObject objectManager;
        [SerializeField]
        [Tooltip("Will autofill with ObjectManager. Drag in object to use in editor")]
        public GameObject managedObject;
        [SerializeField]
        [Tooltip("Will autofill. Drag in object to use in editor")]
        public BoxMaskMaterialAligner cropBox;


        //private
        public PlanSnap[] planSnaps;

        // Use this for initialization
        void Start()
        {
            //try to find missing references
            if (objectManager == null)
            {
                try
                {
                    objectManager = FindObjectOfType<ObjectManager>().gameObject;
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("Unable to find Object Manager script. " +
                        "Either include script, or manually populate object manager field.");
                    //throw;
                }
            }
            if (cropBox == null)
            {
                try
                {
                    cropBox = FindObjectOfType<BoxMaskMaterialAligner>();
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("Unable to find Box Mask Material Aligner script. " +
                        "If using a crop box, either include script, or manually populate Crop Box field.");
                    //throw;
                }
            }
            if (managedObject == null)
            {
                try
                {
                    managedObject = FindObjectOfType<SnappedObject>().gameObject;
                }
                catch (System.Exception)
                {
                    Debug.LogWarning("Unable to find Snapped Object script. " +
                        "If using a crop box, either include script, or manually populate Managed Object field.");
                    //throw;
                }
            }

            //grab all the plan snaps, ALL OF THEM!! (including inactive)
            planSnaps = GetComponentsInChildren<PlanSnap>(true);

            //check for ARKit setup
#if UNITY_IOS
            try
            {
                UnityARCameraManager cameraManager = FindObjectOfType<UnityARCameraManager>();
                if (cameraManager.detectionImages == null)
                {
                    Debug.LogWarning("No reference image set found. Define image set in your Unity AR Camera Manager.");
                }
            }
            catch (System.Exception)
            {
                Debug.LogWarning("No Unity AR Camera Manager found. Your scene doesn't seem to be setup for AR.");
                //throw;
            }
#endif

            //populates collection for turning on/off at activation/deactivation
            planSnaps = GetComponentsInChildren<PlanSnap>(true);

            ResetPlanSnaps();

            //subscribe to ARMarker events
            ARMarker.changeMarker += ClearContent;
            ARMarker.changeMarker += ResetPlanSnaps;
        }

        //unsubscribe from delegate events
        private void OnDestroy()
        {
            ARMarker.changeMarker -= ClearContent;
            ARMarker.changeMarker -= ResetPlanSnaps;
        }

        //get the old stuff out of the way when you see a new sheet
        public void ClearContent()
        {
            if (managedObject)
            {
                Transform managedObjectTransform = managedObject.transform;
                managedObjectTransform.localPosition = new Vector3(0, 2, 0);
                managedObjectTransform.localScale = new Vector3(.01f, .01f, .01f);
            }
            if (cropBox)
            {
                Transform cropBoxTransform = cropBox.transform;
                cropBoxTransform.localPosition = new Vector3(0, 2, 0);
                cropBoxTransform.localScale = new Vector3(.01f, .01f, .01f);
            }
        }

        //Wipes clean any plansnap action going on and sets them all to ready and waiting
        //Should be run at start and every time we switch sheets
        public void ResetPlanSnaps()
        {
            foreach (var item in planSnaps)
            {
                item.StopAR();
            }
        }

    }
}
