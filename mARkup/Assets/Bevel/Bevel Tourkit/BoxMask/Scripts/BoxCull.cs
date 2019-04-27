using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperreal;

public class BoxCull : MonoBehaviour {

    MeshRenderer[] things;

    // Use this for initialization
    void Start () {
        things = GetComponent<BoxMaskMaterialAligner>().TargetGameObject.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var item in things)
        {
            if (item.material.shader.name == "StandardBoxMask" || item.material.shader.name == "StandardBoxMaskTransparent")
            {
                item.gameObject.SetActive(false);
            }
        }
        CullOutsideBox();
     }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CullOutsideBox()
    {
        foreach (var item in things)
        {
            if (item.bounds.Intersects(GetComponent<BoxCollider>().bounds))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }

        }
    }
}
