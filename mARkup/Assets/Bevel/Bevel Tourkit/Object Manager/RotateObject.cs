using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

	public float swipeSpeed = 0.005F;
	public bool rotateAroundCamera = false;

	float inputX;
	float inputY;
	Vector3 rotatePoint;

	// Use this for initialization
	void Start ()
	{
		if (rotateAroundCamera) {
			rotatePoint = Camera.main.transform.position;
		} else {
			rotatePoint = transform.position;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
		//TODO: set a threshold to cause rotation. slow down rotation. 
		//if the threshold is not met, have it do translation. may need large invisible artificial plane.
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			var touchDeltaPosition = Input.GetTouch (0).deltaPosition;						 
			inputX += touchDeltaPosition.x * swipeSpeed;
			inputY += touchDeltaPosition.y * swipeSpeed;
			if (rotateAroundCamera) {
				inputX = -inputX;
			}
			Debug.Log ("input X, Y: " + touchDeltaPosition.x	+ ", " + touchDeltaPosition.y);
			transform.RotateAround (rotatePoint, Vector3.up, -inputX);
		} else {
			inputX = 0;
			inputY = 0;
		}


		//save this part till i test the rotation
		//if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary) {}
		//
	}
}
