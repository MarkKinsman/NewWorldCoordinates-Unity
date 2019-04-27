using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtCamera : MonoBehaviour {

    public bool yAxisOnly = true;
    public bool reverse = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (yAxisOnly)
        {
            transform.LookAt(new Vector3(
                Camera.main.transform.position.x,
                transform.position.y,
                Camera.main.transform.position.z));

            if (reverse)
            {
                transform.Rotate(0f, 180f, 0f);
            }
        }
        else
        {
            transform.LookAt(Camera.main.transform);
        }


	}
}
