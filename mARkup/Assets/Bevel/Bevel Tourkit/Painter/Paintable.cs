using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paintable : MonoBehaviour {

	public Material[] materialLibrary ;
	public Text matLabel;
    public Material hilightMaterial;

	int materialIndex = 0;
	Renderer myRenderer;
	public List<Material> matList;
    GameObject hilight;

	// Use this for initialization
	void Start () {
		myRenderer = gameObject.GetComponent<Renderer> ();
		//fill an empty array with the materials already applied, to avoid errors
		if (materialLibrary.Length == 0) {
			materialLibrary = myRenderer.materials;
		}

        //create the hilight object
        //hilight = Instantiate(gameObject, Vector3.zero, Quaternion.identity, false, transform);
        hilight = new GameObject();
        hilight.name = "Hilight";
        hilight.transform.parent = transform;
        hilight.transform.localPosition = Vector3.zero;
        hilight.transform.localRotation = Quaternion.identity;
        hilight.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        MeshFilter hilightFilter = hilight.AddComponent<MeshFilter>();
        hilightFilter.mesh = GetComponent<MeshFilter>().mesh;
        MeshRenderer hilightRend = hilight.AddComponent<MeshRenderer>();
        hilightRend.material = hilightMaterial;
        hilight.SetActive(false);

        //turn off material label
        try
        {
            matLabel.enabled = false;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("No matlabel found. Exception: " + ex);
        }

		SetMaterial (materialIndex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CycleMaterialUp ()
	{
		Debug.Log ("changing materials");
		materialIndex++;
		if (materialIndex >= materialLibrary.Length) {
			materialIndex = 0;
		}
		SetMaterial (materialIndex);
	}

	public void CycleMaterialDown ()
	{
		Debug.Log ("changing materials");
		materialIndex--;
		if (materialIndex < 0) {
			materialIndex = materialLibrary.Length-1;
		}
		SetMaterial (materialIndex);
	}

	public void SetMaterial(int matIndex){
		SetMaterial (materialLibrary [matIndex]);
	
	}
	public void SetMaterial(Material mat){
		for (int i = 0; i < myRenderer.materials.Length; i++) {
			matList.Add (mat);
			Debug.Log ("Changing material " + i.ToString());
		}
		myRenderer.materials=matList.ToArray();
		matList.Clear ();
		Debug.Log ("New material " + myRenderer.material.name + " is " + mat.name);

		//try to change the label, give up if there isn't one
		try {
			matLabel.text = mat.name;
		} catch (System.Exception ex) {
			Debug.LogError ("No material label found. Exception: " + ex);
		}
	}

	public void Hilight(){
        //turn on hilight object
        hilight.SetActive(true);
        //turn on material label
		try {
			matLabel.enabled = true;
		} catch (System.Exception ex) {
			Debug.LogWarning ("No matlabel found. Exception: " + ex);		
		}
	}
	public void UnHilight(){
        //turn off hilight object
        hilight.SetActive(false);

        //turn off material label
		try {
			matLabel.enabled = false;
		} catch (System.Exception ex) {
			Debug.LogWarning ("No matlabel found. Exception: " + ex);		
		}
	}

}
