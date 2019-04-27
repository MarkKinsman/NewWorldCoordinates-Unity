using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.XR.iOS;
#endif
using Hyperreal;
using Bevel;

/* This is intented to replace the weird thing that the plansnap manager is now. 
 * It will only handle the marker recognition and marker related info/tasks. 
 * The point is to allow wider functionality with the same maker basis.
 * Also more cross-platform
 */

namespace Bevel { 
    public class ARMarker : MonoBehaviour {
#if UNITY_IOS
        [SerializeField]
        private ARReferenceImage referenceImage;
#endif
        public SpriteRenderer sheetSprite;

        bool isCurrent = false;

        static bool firstMarkerFound = false;

        public delegate void FirstMarker();
        public static FirstMarker firstMarker;
        public delegate void ChangeMarker();
        public static ChangeMarker changeMarker;

        public bool showSheetSprite = false;

        private PlanSnapManager planSnapManager;

        public static float managerAlignDelay = 1;



        // Use this for initialization
        void Start ()
        {
            planSnapManager = FindObjectOfType<PlanSnapManager>();
            sheetSprite = GetComponentInChildren<SpriteRenderer>(true);

            //for hiding the picture ofthe sheet where there will be a real sheet.
            if (showSheetSprite)
            {
                sheetSprite.enabled = true;
            }
            else
            {
                sheetSprite.enabled = false;
            }

            //subscribe to ARKit image events
#if UNITY_IOS
            try
            {
                UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
                UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
                UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Failed to subscribe to ARKit image events. " +
                    "Is your build target set? Have you imported and uptodate Unity ARKit Plugin?");
                throw;
            }
#endif

            RemoveAnchor();
        }


        //===============================================


        #region catch AR images anchor recognition and pass to anchor functionality
#if UNITY_IOS
        public void AddImageAnchor(ARImageAnchor arImageAnchor)
        {
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
                Vector3 newPosition = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                Quaternion newRotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
                AddAnchor(newPosition, newRotation);
            }
        }

        public void UpdateImageAnchor(ARImageAnchor arImageAnchor)
        {
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
                Vector3 newPosition = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                Quaternion newRotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
                UpdateAnchor(newPosition, newRotation);
            }
        }

        public void RemoveImageAnchor(ARImageAnchor arImageAnchor)
        {
            if (arImageAnchor.referenceImageName == referenceImage.imageName)
            {
                RemoveAnchor();
            }
        }
#endif
#endregion

        #region Execute anchor moves
        public void AddAnchor(Vector3 newPosition, Quaternion newRotation)
        {
            Debug.Log("image anchor added");

            //invoke delegate events based on conditions.
            if (!isCurrent)
            {
                if (changeMarker != null)
                {
                    changeMarker.Invoke();
                }
            }
            isCurrent = true;
            if (!firstMarkerFound)
            {
                if (firstMarker != null)
                {
                    firstMarker.Invoke();
                }
            }
            firstMarkerFound = true;

            //when you see a new one, get rid of the old ones
            ARMarker[] markers = FindObjectsOfType<ARMarker>();
            foreach (var item in markers)
            {
                if (item != this)
                {
                    item.RemoveAnchor();
                }
            }

            //turn on children
            Transform[] children = GetComponentsInChildren<Transform>(true);
            for (int i = 1; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(true);
            }

            UpdateAnchor(newPosition, newRotation);

            //set inital position 
            PositionObjectManager();

            //if the application is playing, it starts the coroutine that continuously updates object manager position. 
            if (Application.isPlaying)
            {
                Debug.Log("Starting coroutine");
                StartCoroutine("ContinuouslyPosition");
            }
            else
            {
                Debug.Log("No coroutine");
            }
        }


        public void UpdateAnchor(Vector3 newPosition, Quaternion newRotation)
        {
            Debug.Log("image anchor updated");

            if (!isCurrent)
            {
                AddAnchor(newPosition, newRotation);
            }

            transform.position = newPosition;
            transform.rotation = newRotation;
        }

        public void RemoveAnchor()
        {
            Debug.Log("image anchor removed");

            //turn off children
            Transform[] children = GetComponentsInChildren<Transform>();
            for (int i = 1; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(false);
            }

            StopCoroutine("ContinuouslyPosition");

            isCurrent = false;

        }
#endregion

        //unsubscribe from image recongnition events
        void OnDestroy()
        {
#if UNITY_IOS
            UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
            UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;
#endif
        }

        //set the position of the object manager according to the marker location
        public void PositionObjectManager()
        {
            //in case this is running not in runtime, grab the stuff
            if (!planSnapManager)
            {
                planSnapManager = FindObjectOfType<PlanSnapManager>();
            }

            planSnapManager.objectManager.transform.position = transform.position;
            planSnapManager.objectManager.transform.RotateAround(transform.position, Vector3.up,
                transform.rotation.eulerAngles.y - planSnapManager.objectManager.transform.rotation.eulerAngles.y);
            Debug.Log("Parent Euler angles: " + transform.rotation.eulerAngles.ToString());
            Debug.Log("objectmanager euler angles: " + planSnapManager.objectManager.transform.rotation.eulerAngles.ToString());
        }

        //regularly call the Position Object Manager method to keep things cinched.
        IEnumerator ContinuouslyPosition()
        {
            while (true)
            {
                PositionObjectManager();
                yield return new WaitForSeconds(managerAlignDelay);
            }
        }


    }
}