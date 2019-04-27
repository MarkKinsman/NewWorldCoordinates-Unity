using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bevel;

public class SimulateMarkerFind : MonoBehaviour {
    
    public ARMarker armarker;
    public DemoMode demoMode;

	// Use this for initialization
	void Start () {
        armarker = gameObject.GetComponent<ARMarker>();
        demoMode = FindObjectOfType<DemoMode>();

	}
    private void OnEnable()
    {
        Debug.Log(gameObject.name + " enabled. Addin Ancor");
        armarker.UpdateAnchor(demoMode.sheetPlacement.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
