using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMove : MonoBehaviour {

	[Tooltip("Represents the distance traveled (meters) in one full screen height swipe.")]
	public float distanceRatio = 2F;

	float inputX;
	float inputY;
	Vector3 rotatePoint;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

		
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
				//get movement input
			var touchDeltaPosition = Input.GetTouch (0).deltaPosition;						 
			inputY += touchDeltaPosition.y / Screen.height * distanceRatio;
			
				//move elements
			transform.Translate(new Vector3(0, inputY, 0),Space.World);
		} else {
				//reset not moving
			inputX = 0;
			inputY = 0;
		}
			
	}
}
