using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using Bevel;

public class PhotosphereManager : MonoBehaviour {

	public LayerMask ballsLayer;
	public float smallSize = .1f;
	public float largeSize = 40f;
	public float animationTime = 2f;
    [Tooltip("A template on which to base each photosphere material.")]
    public Material basePhotosphereMaterial;
    [Tooltip("The material for the sphere left behind when a selected sphere grows.")]
    public Material selectedSphereMaterial;


	PhotosphereBall[] photospheres;
	PhotosphereBall currentSphere;
	


	//// Use this for initialization
	//void Start () {
	//	//Setup GameObject array by getting children and translating
	//	photospheres = GetComponentsInChildren<PhotosphereBall>(true);

	//}
	

	//public void SetPhotoshpere (PhotosphereBall sphere){
	//	//return all spheres to table
	//	for (int i = 0; i < photospheres.Length; i++) {
	//		photospheres [i].Shrink ();
	//	}
	//	if (currentSphere) {
	//		currentSphere.Invert();
	//	}
			
	//	//setup new selected sphere
	//	currentSphere = sphere;
	//	currentSphere.Grow ();
	//}




}
