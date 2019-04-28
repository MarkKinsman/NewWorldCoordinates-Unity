using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hyperreal;
using UnityEngine.Events;
using Bevel;
#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
using MagicLeap;
#endif

namespace Bevel
{
    public class BevelInput : MonoBehaviour
    {

        public LayerMask defaultClickLayers = ~0;
        public LayerMask defaultCropLayer = 0;

        public bool isClickLogged;

        public bool isPrimaryRayVisualized;
        private LineRenderer primaryRayVis;
        public bool isMLRayVisualized;
        public GameObject rayVisualizer;

        //to discover the main button is released.
        public bool isMainButtonPressed;
        private static bool isMLTriggerDown;

        //or we do them together with a custom class, how have I not thought of this??
        public delegate void ClickEvent(Click click);
        public static ClickEvent clickEvent;
        public static ClickEvent clickEventCropped;

        public static bool subscribedPress;
        public static bool subscribedUnpress;

        public GameObject temporaryClickVis;

        //For isolated test click testing. Not usually called.
        public void ClickedObjectEventNotifier(Click click)
        {
            if (isClickLogged)
            {
                Debug.Log("clicked on " + click.clickedObject.name);
            }
        }

        // Use this for initialization
        void Start()
        {
            clickEvent += ClickedObjectEventNotifier;
#if PLATFORM_LUMIN
            MLInput.OnTriggerDown += SubscribeClickHandler;
            MLInput.OnTriggerUp += SubscribeUnpressHandler;
#endif
            HololensInput.airTapEvent += GenericClick;
        }

        private void OnDestroy()
        {
            clickEvent -= ClickedObjectEventNotifier;
#if PLATFORM_LUMIN
            MLInput.OnTriggerDown -= SubscribeClickHandler;
            MLInput.OnTriggerUp -= SubscribeUnpressHandler;
#endif
            HololensInput.airTapEvent -= GenericClick;
        }


        //My best idea for getting subscribe-based clicks into this whole system. 
        //weird and roundabout but maybe the best way.
        private static void SubscribeClickHandler(byte controllerID, float value)
        {
            subscribedPress = true;
            isMLTriggerDown = true;
        }
        private static void SubscribeUnpressHandler(byte controllerID, float value)
        {
            subscribedUnpress = true;
            isMLTriggerDown = false;
        }

        public void GenericClick()
        {
            Debug.Log("Generic Click");
            subscribedPress = true;
        }
        public void GenericUnclick()
        {
            subscribedUnpress = true;
        }

        // Every frame check for clicks, raycast on clicks, and visualize
        void Update()
        {

            Click click = TestClick(defaultClickLayers);
            if (click != null)
            {
                clickEvent(click);
            }

            Click croppedClick = TestClick(defaultClickLayers, defaultCropLayer);
            if (croppedClick != null)
            {
                clickEventCropped(croppedClick);
            }

#if PLATFORM_LUMIN
            if (isMLRayVisualized)
            {
                visualizeRay(GetControllerRay(), rayVisualizer);
            }
#endif

            if (isPrimaryRayVisualized)
            {
                visualizeRay(GetPrimaryControlRay(), rayVisualizer);
            }

            //always last in update, undo the subscribe handlers
            subscribedPress = false;
        }

        #region (TestClick Called every frame to see if anything is clicked and returns a Click(
        //If no layer is selected, 'Default' layer will be selected. (static version)
        public Click TestClick()
        {
            return TestClick(defaultClickLayers);
        }

        //For use with cropping box. Only allows clicks for objects within the cropbox
        //TODO: extend the delegate version for cropped clicks
        public static Click TestClick(LayerMask testLayer, LayerMask clipLayer)
        {
            RaycastHit hit;
            RaycastHit extendedHit;
            Vector2 clickPosition = new Vector2(0, 0);
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
                            return new Click(extendedHit.collider.gameObject, extendedHit.point); ;
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
        public static Click TestClick(LayerMask layer)
        {

            if (isSomethingPressed())
            {

                Ray clickRay = GetAnyClickRay();

                RaycastHit hit;
                if (Physics.Raycast(clickRay, out hit, 1000f, layer))
                {
                    return new Click(hit.collider.gameObject, hit.point);
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
        #endregion

        //TODO: Might use this to aggregate controller tests. Might not be worth it. 
        //Maybe should just have one for each platform. But for now.
        
        public static bool isSomethingPressed()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
            else if (subscribedPress)
            {
                return true;
            }
            return false;
        }

        public static bool isSomethingStillPressed()
        {
            if (Input.GetMouseButton(0))
            {
                return true;
            }
            else if (Input.touchCount == 1)
            {
                return true;
            }
            else if (isMLTriggerDown)
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
            else if (subscribedPress)
            {
                subscribedPress = false;
                clickRay = GetCameraRay();

                            #if PLATFORM_LUMIN
                clickRay = GetControllerRay();
#endif
                return clickRay;
            }
            else
            {
                Debug.LogError("No click found. Improperly called function. Ray returned will be false.");
                return clickRay;
            }
        }

        public static Ray GetPrimaryControlRay()
        {
#if PLATFORM_LUMIN
            if (GameObject.FindObjectOfType<ControllerTransform>())
            {
                return GetControllerRay();
            }
#endif
            if (Input.mousePresent)
            {
                return GetMouseRay();
            }
            else
            {
                return GetCameraRay();
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
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            return cameraRay;
        }
        public static Ray GetControllerRay()
        {
            GameObject controller = new GameObject();
#if UNITY_IOS
            controller = Camera.main.gameObject;
#endif
#if PLATFORM_LUMIN
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
                objectAtHit.transform.eulerAngles = hit.normal;

                primaryRayVis = GetComponent<LineRenderer>();
                primaryRayVis.enabled = true;
                Vector3[] visPoints = new Vector3[2];
                visPoints[0] = ray.origin;
                visPoints[1] = hit.point;
                primaryRayVis.SetPositions(visPoints);
            }
            else
            {
                objectAtHit.SetActive(false);
                primaryRayVis.enabled = false;
            }

        }
    }


    //so that I can return all the data I want
    public class Click
    {
        private bool isValid;
        public GameObject clickedObject;
        public Vector3 clickPoint;

        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }

        public Click()
        {
            isValid = false;
        }

        public Click(GameObject setObject, Vector3 setPoint)
        {
            clickedObject = setObject;
            clickPoint = setPoint;
            isValid = true;
        }
    }
}