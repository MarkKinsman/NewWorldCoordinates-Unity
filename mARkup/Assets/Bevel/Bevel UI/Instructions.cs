using System.Collections;
using System.Collections.Generic;
using Bevel;
using UnityEngine;

public class Instructions : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        ARMarker.firstMarker += RemoveInstructions;
    }

    void OnDestroy()
    {
        ARMarker.firstMarker -= RemoveInstructions;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveInstructions()
    {
        gameObject.SetActive(false);
    }

}
