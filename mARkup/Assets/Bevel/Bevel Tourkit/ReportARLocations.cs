using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportARLocations : MonoBehaviour {

	public Text cameraLocationReport;
	public Text objectManagerReport;
	public Text objectManagerLocalReport;
	public Text deltaPositionReport;



	Transform mainCameraTransform;
	GameObject objectManagerObject;

	// Use this for initialization
	void Start () {
		mainCameraTransform = Camera.main.transform;
		objectManagerObject = GameObject.FindObjectOfType<ObjectManager> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		cameraLocationReport.text = mainCameraTransform.position.ToString ();
		objectManagerReport.text = objectManagerObject.transform.position.ToString ();
		objectManagerLocalReport.text = objectManagerObject.transform.localPosition.ToString ();

	}
}
