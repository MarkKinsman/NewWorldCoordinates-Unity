using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Painter : MonoBehaviour {

	public GameObject paintableThing;

	public float maxRayDistance = 30f;
	public LayerMask collisionLayerMask;
	public GameObject paintButton;

	Paintable paintable;
	Vector2 touchPosition;
	Vector2 screenCenter;
	Vector3 screenPoint;
	Vector3 lastHitLocation;
	Ray touchRay;
	Ray centerRay;
	RaycastHit hit;


	// Use this for initialization
	void Start () {
		screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);
		paintButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
		centerRay = Camera.main.ScreenPointToRay (screenCenter);
		if (Physics.Raycast (centerRay, out hit, maxRayDistance, collisionLayerMask)) {
			paintButton.SetActive (true);
			//do the mat thing
			if (paintableThing == null) {
				paintableThing = hit.collider.gameObject;
				try {
					paintable = paintableThing.GetComponent<Paintable> ();
					paintable.Hilight ();
				} catch (System.Exception ex) {
					Debug.Log ("Paintable not found. Exception: " + ex);
				}

				Debug.Log ("it hit " + paintableThing.name);

			}

		} else {
			paintButton.SetActive (false);
			if (paintableThing != null) {
				Debug.Log ("it didn't hit");

				try {
					paintable = paintableThing.GetComponent<Paintable> ();
					paintable.UnHilight ();
				} catch (System.Exception ex) {
					Debug.Log ("Paintable not found. Exception: " + ex);
				}
				paintableThing = null;

			}

			//click to cycle up materials
			if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began) {
				touchPosition = Input.GetTouch (0).position;						 
				screenPoint = new Vector2 (touchPosition.x, touchPosition.y);
				touchRay = Camera.main.ScreenPointToRay (screenPoint);

				SelectPaintable (touchRay);
		
			} 
		}
	}

	public void SelectPaintable(Ray ray){
		if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayerMask)) {
			paintable.CycleMaterialUp();
		}
	}

	public void CycleMaterial ()
	{
		paintable.CycleMaterialUp ();

	}

}
