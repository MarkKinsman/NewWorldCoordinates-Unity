using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hyperreal;
using UnityEngine.Events;
using Bevel;
#if UNITY_LUMIN
using UnityEngine.XR.MagicLeap;
using MagicLeap;
#endif

namespace Bevel
{
    public class BevelInput : MonoBehaviour {

        public LayerMask defaultClickLayers = ~0;
        public LayerMask defaultCropLayer = 0;
        public LayerMask defaultButtonLayer = 11;

        public bool isClickLogged;

        public bool isMLRayVisualized;
        public GameObject rayVisualizer;

        public delegate void ClickObjectEvent(GameObject clickedObject);
        public static ClickObjectEvent clickObjectEvent;
        public static ClickObjectEvent clickObjectEventCropped;
        public static ClickObjectEvent clickButtonEvent;
        public static ClickObjectEvent clickButtonEventCropped;

        public delegate void ClickHitEvent(RaycastHit clickHit);
        public ClickHitEvent clickHitEvent;

        public static bool subscribedClick;

        public GameObject temporaryClickVis;

        //For isolated test click testing. Not usually called.
        public void ClickedObjectEventNotifier(GameObject thing)
        {
            if (isClickLogged)
            {
                Debug.Log("clicked on " + thing.name);
            }
        }

        // Use this for initialization
        void Start ()
        {
            clickObjectEvent += ClickedObjectEventNotifier;
#if UNITY_LUMIN
            MLInput.OnTriggerDown += SubscribeClickHandler;
#endif
        }

        private void OnDestroy()
        {
            clickObjectEvent -= ClickedObjectEventNotifier;
#if UNITY_LUMIN
            MLInput.OnTriggerDown -= SubscribeClickHandler;
#endif
        }


        //My best idea for getting subscribe-based clicks into this whole system. 
        //weird and roundabout but maybe the best way.
        private static void SubscribeClickHandler(byte controllerID, float value)
        {
            subscribedClick = true;
        }
	
	    // Every frame check for clicks, raycast on clicks, and visualize
	    void Update () {
            GameObject clickedObject;
            if (clickedObject = TestClick(defaultClickLayers))
            {
                clickObjectEvent(clickedObject);
            }
            if (clickedObject = TestClick(defaultClickLayers, defaultCropLayer))
            {
                clickObjectEventCropped(clickedObject);
            }
            if (clickedObject = TestClick(defaultButtonLayer))
            {
                clickButtonEvent(clickedObject);
            }
            if (clickedObject = TestClick(defaultButtonLayer, defaultCropLayer))
            {
                clickButtonEventCropped(clickedObject);
            }


#if UNITY_LUMIN
            if (isMLRayVisualized)
            {
                visualizeRay(GetControllerRay(), rayVisualizer);
            }
#endif

            //always last in update, undo the subscribe handlers
            subscribedClick = false;
        }

        //If no layer is selected, 'Default' layer will be selected. (static version)
        public GameObject TestClick()
        {
            return TestClick(defaultClickLayers);
        }


        //For use with cropping box. Only allows clicks for objects within the cropbox
        //TODO: extend the delegate version for cropped clicks
        public static GameObject TestClick(LayerMask testLayer, LayerMask clipLayer)
        {
            RaycastHit hit;
            RaycastHit extendedHit;
            Vector2 clickPosition = new Vector2(0,0);
            bool clicked = false;
            //test multitouch
        
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                clickPosition = Input.GetTouch(0).position;
                clicked = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                clickPosition = Input.mousePosition;
                clicked = true;
            }


            if (clicked)
            {
                Vector3 clickPoint = new Vector3(clickPosition.x, clickPosition.y, 1f);
                Ray clickRay = Camera.main.ScreenPointToRay(clickPoint);

                if (Physics.Raycast(clickRay, out hit, 1000f, clipLayer))
                {
                    //create a new ray that extends from the hitpoint of the clipping box
                    Ray newRay = new Ray(hit.point, clickRay.direction);
                    if (Physics.Raycast(newRay, out extendedHit, 10000f, testLayer))
                    {
                        if (FindObjectOfType<BoxMaskMaterialAligner>().GetComponent<Collider>().bounds.Contains(extendedHit.point))
                        {
                            return extendedHit.collider.gameObject;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        Debug.Log("Entered cropping bounds but did not hit object");
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }


        //for both mouse and ios, test for clicks
        public static GameObject TestClick(LayerMask layer){
            
            if (isSomethingClicked())
            {
                
                Ray clickRay = GetAnyClickRay();

                RaycastHit hit;
                if (Physics.Raycast(clickRay, out hit, 1000f, layer))
                {
                    return hit.collider.gameObject;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }



        //TODO: Might use this to aggregate controller tests. Might not be worth it. 
        //Maybe should just have one for each platform. But for now.
        public static bool isSomethingClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
            else if (subscribedClick)
            {
                return true;
            }
                return false;
        }

        //make this a little more extensible. Get rays from each device.Then use them.
        public static Ray GetAnyClickRay()
        {
            Ray clickRay = new Ray();
            if (Input.GetMouseButtonDown(0))
            {
                clickRay = GetMouseRay();
                return clickRay;
            }
            else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                clickRay = GetTouchRay();
                return clickRay;
            }
            else if (subscribedClick)
            {

                subscribedClick = false;
                clickRay = GetControllerRay();
                return clickRay;
            }
            else
            {
                Debug.LogError("No click found. Improperly called function. Ray returned will be false.");
                return clickRay;
            }
        }

#region Get Rays
        public static Ray GetMouseRay()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 mousePoint = new Vector3(mousePosition.x, mousePosition.y, 1f);
            Ray mouseRay = Camera.main.ScreenPointToRay(mousePoint);
            return mouseRay;
        }
        public static Ray GetTouchRay()
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            Vector3 touchPoint = new Vector3(touchPosition.x, touchPosition.y, 1f);
            Ray touchRay = Camera.main.ScreenPointToRay(touchPoint);
            return touchRay;
        }
        public static Ray GetCameraRay()
        {
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward) ;
            return cameraRay;
        }
        public static Ray GetControllerRay()
        {
            GameObject controller = new GameObject();
#if UNITY_IOS
            controller = Camera.main.gameObject;
#endif
#if UNITY_LUMIN
            controller = GameObject.FindObjectOfType<ControllerTransform>().gameObject;
#endif
            Ray controllerRay = new Ray(controller.transform.position, controller.transform.forward);

            return controllerRay;
        }
        public Ray GetArmRay()
        {
            Ray ray = new Ray();
            return ray;
        }
#endregion

        //stick an object at the ray hit point so we can see that it's working.
        public void visualizeRay(Ray ray, GameObject objectAtHit)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, defaultClickLayers))
            {
                objectAtHit.SetActive(true);
                objectAtHit.transform.position = hit.point;
            }
            else
            {
                objectAtHit.SetActive(false);
            }

        }
    }
}