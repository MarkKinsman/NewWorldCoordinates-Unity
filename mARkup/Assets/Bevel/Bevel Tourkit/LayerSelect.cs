using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySelect : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//TODO: change this to modifying their visibility, but keep the object itself in the scene.
	public void TurnOnObject (GameObject thing)
	{
		thing.SetActive (!thing.activeInHierarchy);
	}

}
