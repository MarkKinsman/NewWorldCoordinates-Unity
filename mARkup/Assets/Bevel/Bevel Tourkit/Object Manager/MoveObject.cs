using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

	public float maxRayDistance = 30f;
	public LayerMask collisionLayerMask;

	Vector2 touchPosition;
	Vector3 screenPoint;
	Vector3 lastHitLocation;
	Ray ray;
	RaycastHit hit;
	//Vector3 deltaHit;

	//add this if I have trouble moving stuff
	//float lowestPlaneHeight

	// Use this for initialization
	void Start ()
	{
		touchPosition = new Vector2 (Screen.width / 2, Screen.height / 2);
		screenPoint = new Vector3 (touchPosition.x, touchPosition.y, 1f);

	}
	
	// Update is called once per frame
	void Update ()
	{
		//TODO: set a threshold to cause rotation. slow down rotation. 
		//if the threshold is not met, have it do translation. may need large invisible artificial plane.
		if (Input.touchCount == 1) {
			touchPosition = Input.GetTouch (0).position;
		}
								 
		screenPoint = new Vector3 (touchPosition.x, touchPosition.y, 1f);
		ray = Camera.main.ScreenPointToRay (screenPoint);



		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began) {
			var deltaHit = GetDeltaHit ();
		} else if (Input.touchCount == 1) {
			var deltaHit = GetDeltaHit ();
			transform.Translate (new Vector3 (deltaHit.x, 0, deltaHit.z), Space.World);
		} 

		/* else {
			if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayerMask)) {
				Vector3 deltaHit = hit.point - lastHitLocation;
				lastHitLocation = hit.point;
			}
		} 
		*/


	}

	Vector3 GetDeltaHit ()
	{
		if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayerMask)) {
			Vector3 deltaHit = hit.point - lastHitLocation;
			lastHitLocation = hit.point;
			return deltaHit;
		} else {
			Debug.LogWarning ("No Plane found in move command");
			return Vector3.zero;
		}

	}

}
