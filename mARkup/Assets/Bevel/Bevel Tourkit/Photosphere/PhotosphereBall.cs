using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_IOS
using UnityEngine.iOS;
#endif


//TODO: Setup custom inspector to save rotation info. 

public class PhotosphereBall : MonoBehaviour {

    [Tooltip("This will eventually replace creating the materials manually")]
    public Texture photosphereImage;
    [Tooltip("Optional reduced size version of the photosphere image to improve " +
             "performance when many are shown in small form. If no thumbnail is provided, " +
             "script will attempt to create one.")]
    public Texture photosphereThumbnail;

    //overrides
    [Tooltip("Enter value to override manager. Default value is .05 for scale model or plan.")]
    public float smallSizeOverride;
    [Tooltip("Enter value to override manager. Default value is 40 for scale model or plan.")]
    public float largeSizeOverride;
    [Tooltip("Enter value to override manager. Default value is 2 for scale model or plan.")]
    public float animationTimeOverride;
    [Tooltip("A template on which to base each photosphere material.")]
    public Material basePhotosphereMaterialOverride;
    [Tooltip("The material for the sphere left behind when a selected sphere grows.")]
    public Material selectedSphereMaterialOverride;




    PhotosphereManager photosphereManager;
    private float smallSize;
    private float largeSize;
    private float animationTime;
    private Material basePhotosphereMaterial;
    private Material selectedSphereMaterial;
    private Material smallMaterial;
    private Material largeMaterial;
    private GameObject selectedSphere;
    private bool isCurrentSphere;

    //Use this to make stuff
    private void Awake()
    {
        //Create sphere to leave behind when this sphere grows.
        selectedSphere = new GameObject();
        selectedSphere.transform.parent = transform.parent;
    }

    // Use this for initialization
    void Start () {
        gameObject.name = photosphereImage.name;
		photosphereManager = FindObjectOfType<PhotosphereManager> ();

        //apply overrides or get info from manager
        if (smallSizeOverride == 0)
        {
            if (photosphereManager)
            {
                smallSize = photosphereManager.smallSize;
            }
            else
            {
                smallSize = .1f;
            }
        }
        else
        {
            smallSize = smallSizeOverride;
        }
        if (largeSizeOverride == 0f)
        {
            if (photosphereManager)
            {
                largeSize = photosphereManager.largeSize;
            }
            else
            {
                largeSize = 40f;
            }
        }
        else
        {
            largeSize = largeSizeOverride;
        }
        if (animationTimeOverride == 0f)
        {
            if (photosphereManager)
            {
                animationTime = photosphereManager.animationTime;
            }
            else
            {
                animationTime = 2f;
            }
        }
        else
        {
            animationTime = animationTimeOverride;
        }
        if (basePhotosphereMaterialOverride == null)
        {
            if (photosphereManager)
            {
                basePhotosphereMaterial = photosphereManager.basePhotosphereMaterial;
            }
            else
            {
                basePhotosphereMaterial = new Material(Shader.Find("Standard"));
            }
        }
        else
        {
            basePhotosphereMaterial = basePhotosphereMaterialOverride;
        }
        if (selectedSphereMaterialOverride == null)
        {
            if (photosphereManager)
            {
                selectedSphereMaterial = photosphereManager.selectedSphereMaterial;
            }
            else
            {
                selectedSphereMaterial = new Material(Shader.Find("Standard"));
                selectedSphereMaterial.color = new Color(.5f, 1, 1, .5f);
            }
        }
        else
        {
            selectedSphereMaterial = selectedSphereMaterialOverride;
        }



        //setup materials
        largeMaterial = GenerateSphereMaterial(basePhotosphereMaterial, photosphereImage);
        if (photosphereThumbnail != null)
        {
            smallMaterial = GenerateSphereMaterial(basePhotosphereMaterial, photosphereThumbnail);
        }
        else 
        {
            try
            {
                photosphereThumbnail = GenerateThumbnail(photosphereImage);
                smallMaterial = GenerateSphereMaterial(basePhotosphereMaterial, photosphereThumbnail);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning(photosphereImage.name + " is not read/write enabled. " +
                                 "Defaulting to large image on both large and small spheres. " +
                                 "This will hurt performance. Please go to import settinss and " +
                                 "'enable read/write' under 'Advanced'. System Exception: " + ex);
                smallMaterial = GenerateSphereMaterial(basePhotosphereMaterial, photosphereImage);
            }
             
        }

        //setup selected sphere stuff
        selectedSphere.name = gameObject.name + " Selected";
        selectedSphere.transform.position = transform.position;
        selectedSphere.transform.localScale = new Vector3(smallSize * .9f, smallSize * .9f, smallSize * .9f);
        MeshFilter meshFilter = selectedSphere.AddComponent<MeshFilter>();
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        MeshRenderer meshRenderer = selectedSphere.AddComponent<MeshRenderer>();
        meshRenderer.material = selectedSphereMaterial;
        selectedSphere.AddComponent<SphereCollider>();
        selectedSphere.layer = gameObject.layer;
        //make button on selected sphere to close it
        selectedSphere.AddComponent<SpatialButton>();
        SpatialButton selectedSphereButton = selectedSphere.GetComponent<SpatialButton>();
        selectedSphereButton.calledOnClick.AddListener(DeselectSphere);

        //sets the current sphere to the small material
        GetComponent<MeshRenderer>().material = smallMaterial;

        //apply default settings to all the balls
        foreach (var item in FindObjectsOfType<PhotosphereBall>())
        {
            item.Shrink();
        }

	}

    public void ToggleSphere()
    {
        if (isCurrentSphere)
        {
            DeselectSphere();
        }
        else
        {
            SelectSphere();
        }
    }

    //Call this when clicking on a sphere to select it, or if selected already, turn it off
    public void SelectSphere()
    {
        //make sure everything is off before turning one on
        foreach (var item in FindObjectsOfType<PhotosphereBall>())
        {
            if (item.isCurrentSphere)
            {
                item.DeselectSphere();
            }
        }

        Grow();
        Invert();
        isCurrentSphere = true;
    }

    public void DeselectSphere()
    {
        if (isCurrentSphere)
        {
            Shrink();
            Invert();
        }

        isCurrentSphere = false;
    }

    public void Grow(){
		//Lerp to grow the selected sphere
		StopCoroutine ("GrowSphere");
		StartCoroutine("GrowSphere");

		GetComponent<MeshRenderer> ().material = largeMaterial;
	}

	public void Shrink(){
        StopCoroutine("GrowSphere");
		transform.localScale = new Vector3 (smallSize, smallSize, smallSize);
		GetComponent<MeshRenderer> ().material = smallMaterial;
	}

	//create mesh variable, invert it and assign it to mesh
	public void Invert(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.triangles = mesh.triangles.Reverse().ToArray();
	}

	//Gradually grow a sphere
	private IEnumerator GrowSphere(){
		float currentSize = smallSize;
		float t = 0f;
		while (currentSize<largeSize) {
			t += Time.deltaTime / animationTime;
			currentSize = smallSize * Mathf.Lerp (1, largeSize/smallSize, t);
			transform.localScale = new Vector3 (-currentSize, currentSize, currentSize);
			yield return null;
		}

	}

	//Gradually grow a sphere (this version doesn't work)
	private IEnumerator GrowSphereFancy(){
		float currentSize = smallSize;
		float increment = Mathf.Pow(largeSize / smallSize, 1 / (animationTime / Time.deltaTime));
		while (currentSize<largeSize) {
			currentSize = currentSize * increment;
			transform.localScale = new Vector3 (-currentSize, currentSize, currentSize);
			yield return null;
		}
	}

    public Material GenerateSphereMaterial(Material baseMaterial, Texture texture){
        Material material = new Material(source:baseMaterial);
        material.mainTexture = photosphereImage;
        material.SetTexture("_EmissionMap", texture);
        material.name = texture.name;
        return material;
    }

    public Texture GenerateThumbnail(Texture originalTexture){
        Texture2D original2D = originalTexture as Texture2D;
        Texture2D thumbnailTex = new Texture2D(originalTexture.width / 4, originalTexture.height / 4);
        thumbnailTex.SetPixels(original2D.GetPixels(1));
        thumbnailTex.Apply();
        thumbnailTex.name = originalTexture.name + " thumbnail";
        //byte[] png = temp.EncodeToPng();
        //Destroy(temp);
        //originalTexture.LoadImage(png);
        return thumbnailTex;
    }

}
