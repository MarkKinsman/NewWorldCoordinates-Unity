using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderTest : MonoBehaviour {

    public GameObject shoulderPrefab;
    public Vector3 shoulderOffset = new Vector3(.1f, -.1f, 0);

    GameObject rightShoulder;
    GameObject leftShoulder;
    Transform cameraTrans;

	// Use this for initialization
	void Start () {
        cameraTrans = Camera.main.transform;

        

        rightShoulder = Instantiate(shoulderPrefab, cameraTrans.position + shoulderOffset, cameraTrans.rotation, cameraTrans);
        rightShoulder.name = "Right Shoulder";

        leftShoulder = Instantiate(shoulderPrefab, cameraTrans.position + new Vector3(-shoulderOffset.x, shoulderOffset.y, shoulderOffset.z), cameraTrans.rotation, cameraTrans);
        leftShoulder.name = "Left Shoulder";
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
