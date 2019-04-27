using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class BevelWeb : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenLink(string url){
        Application.OpenURL(url);
    }

}
