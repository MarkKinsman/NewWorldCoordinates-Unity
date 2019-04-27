using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Bevel;

/* This script creates the point of comparison for saving all other locations in AR. 
 * Also serves as the Anchor manager. Saving all anchor info. 
 * If using for literal mapping point, place aligned to the nearest survey bench-mark.
 * Otherwise, just stick it at the origin and let it do its thing. 
 */
 namespace Bevel
{
    public class LocationManager : MonoBehaviour
    {
        [Tooltip("Latitude in decimal degrees. For locating in the world. Not required for AR persistence.")]
        [SerializeField] private double latitude = 45.506663;
        [Tooltip("Longitude in decimal degrees. For locating in the world. Not required for basic AR persistence.")]
        [SerializeField] private double longitude = -122.655625;
        [Tooltip("Elevation in decimal meters. For locating in the world. Not required for basic AR persistence.")]
        [SerializeField] private double elevation = 14.6304;

        [SerializeField] private Anchor[] anchors;

        //Control for singleton. Overkill? meh.
        public static LocationManager locationManager;
        void Awake()
        {
            if (locationManager == null)
            {
                DontDestroyOnLoad(gameObject);
                locationManager = this;
            }
            else if (locationManager != this)
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            anchors = GameObject.FindObjectsOfType<Anchor>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        //cycle through saved Anchors and save transform and placement info to the binary file.
        public void SaveAnchorState()
        {
            Debug.Log("saving ");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/anchorInfo.dat");


            AnchorData[] anchorData = new AnchorData[anchors.Length];
            for (int i = 0; i < anchorData.Length; i++)
            {
                anchorData[i] = new AnchorData(anchors[i].gameObject.name,anchors[i].currentState, anchors[i].AnchorPosition, anchors[i].AnchorRotation);
                //anchorData[i].name = anchors[i].gameObject.name;
                //anchorData[i].anchorState = anchors[i].currentState;
                if (anchorData[i].anchorState == Anchor.AnchorState.Placed)
                {
                    anchorData[i].timePlaced = anchors[i].TimePlaced;

                    //anchorData[i].anchorPositionX = anchors[i].AnchorPosition.x;
                    //anchorData[i].anchorPositionY = anchors[i].AnchorPosition.y;
                    //anchorData[i].anchorPositionZ = anchors[i].AnchorPosition.z;

                    //anchorData[i].anchorRotationX = anchors[i].AnchorRotation.x;
                    //anchorData[i].anchorRotationY = anchors[i].AnchorRotation.y;
                    //anchorData[i].anchorRotationZ = anchors[i].AnchorRotation.z;
                }
            }
            bf.Serialize(file, anchorData);
        }

        //cycle through saved Anchors and load transform and placement info from the binary file.
        public void LoadAnchorState()
        {
            Debug.Log("loading...");
            if (File.Exists(Application.persistentDataPath + "/anchorInfo.dat"))
            {
                //open file and pull info out
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/anchorInfo.dat", FileMode.Open);
                AnchorData[] anchorData = (AnchorData[])bf.Deserialize(file);
                file.Close();

                int placedCount = 0;
                foreach (var anchor in anchorData)
                {
                    Debug.Log(String.Format("Anchor {0} position: {1}", anchor.name, anchor.anchorState));
                    //only pull this info and set the anchor if saved anchor is placed.
                    if (anchor.anchorState == Anchor.AnchorState.Placed)
                    {

                        
                        Debug.Log(String.Format("Anchor {0} position: {1}, {2}, {3}", 
                            anchor.name, anchor.anchorPositionX, anchor.anchorPositionX, anchor.anchorPositionX));
                        Debug.Log(String.Format("Anchor {0} rotation: {1}, {2}, {3}", 
                            anchor.name, anchor.anchorRotationX, anchor.anchorRotationY, anchor.anchorRotationZ));

                        placedCount++;
                    }
                }
                Debug.Log( string.Format("Loaded {0} anchors. {1} anchors were placed.", anchorData.Length, placedCount));
            }
        }
    }

    [Serializable]
    public class AnchorData
    {
        public string name;
        public Anchor.AnchorState anchorState;
        public DateTime timePlaced;

        public float anchorPositionX;
        public float anchorPositionY;
        public float anchorPositionZ;
        public float anchorRotationX;
        public float anchorRotationY;
        public float anchorRotationZ;

        public AnchorData()
        {

        }
        public AnchorData(string name, Anchor.AnchorState anchorState)
        {
            this.name = name;
            this.anchorState = anchorState;
        }
        public AnchorData(string name, Anchor.AnchorState anchorState, Vector3 anchorPosition, Vector3 anchorRotation)
        {
            this.name = name;
            this.anchorState = anchorState;

            anchorPositionX = anchorPosition.x;
            anchorPositionY = anchorPosition.y;
            anchorPositionZ = anchorPosition.z;
            anchorRotationX = anchorRotation.x;
            anchorRotationY = anchorRotation.y;
            anchorRotationZ = anchorRotation.z;

        }
    }
}

