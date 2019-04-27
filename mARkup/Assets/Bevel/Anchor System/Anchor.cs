using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_LUMIN
using UnityEngine.XR.MagicLeap;
#endif
using Bevel;


namespace Bevel
{
    public class Anchor : MonoBehaviour
    {
        public bool isFound;
        public enum AnchorState { Available = 0, Placed =1, Adjusting =2};
        [SerializeField] public AnchorState currentState = AnchorState.Available;

        private LocationManager locationManager;

        //References
#if UNITY_LUMIN
        private MLImageTrackerBehavior _trackerBehavior;
#endif
        public GameObject content;
        public GameObject _availableMenu;
        public GameObject _placedMenu;

        //private variables
        [SerializeField] private Vector3 savedPosition;
        [SerializeField] private Vector3 savedRotation;
        [SerializeField] private DateTime timePlaced;

        private Vector3 relativePosition;
        private Vector3 relativeRotation;


        public Vector3 AnchorPosition
        {
            get
            {
                return savedPosition;
            }

            set
            {
                savedPosition = value;
            }
        }
        public Vector3 AnchorRotation
        {
            get
            {
                return savedRotation;
            }

            set
            {
                savedRotation = value;
            }
        }
        public DateTime TimePlaced
        {
            get
            {
                return timePlaced;
            }

            set
            {
                timePlaced = value;
            }
        }

#region Stanard Unity Methods
        // Use this for initialization
        void Start()
        {
#if UNITY_LUMIN
            _trackerBehavior = gameObject.GetComponent<MLImageTrackerBehavior>();
            locationManager = FindObjectOfType<LocationManager>();

            //subscribe anchor methods
            //_trackerBehavior.OnTargetFound += TargetFoundHandler;
            //_trackerBehavior.OnTargetLost += OnLoseTarget;
            _trackerBehavior.OnTargetUpdated += UpdateTargetHandler;
#endif

            _placedMenu.SetActive(false);
            _availableMenu.SetActive(false);
            if (Application.isEditor)
            {
                GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
            }
            
        }
        private void OnDestroy()
        {
            #if UNITY_LUMIN
            //_trackerBehavior.OnTargetFound -= TargetFoundHandler;
            //_trackerBehavior.OnTargetLost -= OnLoseTarget;
            _trackerBehavior.OnTargetUpdated -= UpdateTargetHandler;
#endif
        }

        // Update is called once per frame
        void Update()
        {

        }
#endregion

#region Marker vision handling
        //subscribe to target find event or call from the editor seeing simulator button
        public void TargetFoundHandler(bool isReliable)
        {
            Debug.Log("Anchor " + gameObject.name + " was found");
            isFound = true;

            //initializes menu visibility
            UpdateState();
            if (currentState == AnchorState.Placed)
            {
                //find the difference between where the target is vs where it should be.
                Vector3 deltaPositon = AnchorPosition - transform.position;
                Vector3 deltaRotation = AnchorRotation - transform.rotation.eulerAngles;

                //move player and anchor according to there delta transform (player first to avoid loss of reference
                content.transform.Translate(-deltaPositon, Space.World);
                content.transform.RotateAround(transform.position, Vector3.up, -deltaRotation.y);
            }
        }


        //receives the result from the tracking and passes to the update executor
#if UNITY_LUMIN

        public void UpdateTargetHandler(MLImageTargetResult result)
        {
            Vector3 trackedPosition = result.Position;
            Quaternion trackedRotation = result.Rotation;
            MLImageTargetTrackingStatus trackedStatus = result.Status;

            if (result.Status == MLImageTargetTrackingStatus.Tracked)
            {
                UpdateTargetExecutor(result.Position, result.Rotation);
                //initializes menu visibility
                UpdateState();
            }
            else
            {
                OnLoseTarget();
            }

        }
#endif
        public void UpdateTargetSimulator()
        {
            UpdateTargetExecutor(transform.position, transform.rotation);
        }

        //pulled functionality into this method so we could simulate call updating a target
        public void UpdateTargetExecutor(Vector3 markerPosition, Quaternion markerRotation)
        {
            if (currentState == AnchorState.Placed)
            {
                //mar?

                //move the location manager marker parent into place for the local positions to be correct.
                Vector3 newCentralRotation = markerRotation.eulerAngles - AnchorRotation;
                Vector3 newCentralPostition = markerPosition - AnchorPosition;
                locationManager.transform.SetPositionAndRotation(newCentralPostition, Quaternion.Euler(newCentralRotation));
                content.transform.SetPositionAndRotation(newCentralPostition, Quaternion.Euler(newCentralRotation));

                //restore local position of this marker
                transform.localRotation = Quaternion.Euler(AnchorRotation);
                transform.localPosition = AnchorPosition;
            }
        }

        //the old update executor will remove when new method is proven
        public void MoveStuff(Vector3 markerPosition, Quaternion markerRodation)
        {
            //find the difference between where the target is vs where it should be.
            Vector3 deltaPositon = markerPosition - relativePosition;
            Vector3 deltaRotation = markerRodation.eulerAngles - relativeRotation;

            if (currentState == AnchorState.Placed)
            {
                //move player and anchor according to there delta transform (player first to avoid loss of reference
                content.transform.Translate(deltaPositon, Space.World);
                content.transform.RotateAround(transform.position, Vector3.up, deltaRotation.y);
            }
        }


        //subscribe to target lose event or call from the editor don't see simulator button
        public void OnLoseTarget()
        {
            Debug.Log("Anchor " + gameObject.name + " was lost from view");
            _placedMenu.SetActive(false);
            _availableMenu.SetActive(false);
            isFound = false;
        }
#endregion //Move to platform-specific script that calls this one

#region Placement Region
        public void Place()
        {
            TimePlaced = DateTime.Now;

            //Save current position of anchor to be a source later
            AnchorPosition = transform.localPosition;
            AnchorRotation = transform.localRotation.eulerAngles;

            //update according to change
            currentState = AnchorState.Placed;
            UpdateState();
        }
        public void UnPlace()
        {
            //update according to change
            currentState = AnchorState.Available;
            UpdateState();

        }
        public void AdjustPlacement()
        {
            //this will want some clever stuff for moving the model

            //update according to change
            currentState = AnchorState.Adjusting;
            UpdateState();
        }
#endregion



        //run setup according to if the thing is placed or not.
        void UpdateState()
        {
            if (currentState == AnchorState.Available)
            {
                _availableMenu.SetActive(true);
                _placedMenu.SetActive(false);
            }
            else if (currentState == AnchorState.Placed)
            {
                _placedMenu.SetActive(true);
                _availableMenu.SetActive(false);
            }
            else if (currentState == AnchorState.Adjusting)
            {
                UnPlace();
            }
        }

        //For prebaking markers into the scene
        public void PrebakePlacement()
        {
            savedPosition = transform.localPosition;
            savedRotation = transform.localEulerAngles;

            currentState = AnchorState.Placed;
            UpdateState();
        }

        //load anchordata from a previous session
        public void LoadAnchor(AnchorData datum)
        {
            currentState = datum.anchorState;
            savedPosition = new Vector3(datum.anchorPositionX, datum.anchorPositionY, datum.anchorPositionZ);
            AnchorRotation = new Vector3(datum.anchorRotationX, datum.anchorRotationY, datum.anchorRotationZ);
            
        }

        //try placing the player at a world location based on the position of the marker relative to the location manager
        //maybe account for change in player position. Markers and marker visualizers. 
        public void NewLocate()
        {

        }


    }
}
