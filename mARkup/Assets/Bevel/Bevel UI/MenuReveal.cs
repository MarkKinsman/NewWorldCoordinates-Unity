using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuReveal : MonoBehaviour {

	public GameObject menu;

	bool menuIsOn = false;

	// Use this for initialization
	void Start () {
		
		menuIsOn = menu.activeInHierarchy;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleMenu(){
		Debug.Log ("Toggling Menu");
		menuIsOn = !menuIsOn;
		menu.SetActive (menuIsOn);
	}

}
