using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bevel;

public class DemoMode : MonoBehaviour {

    public GameObject tablePrefab;
    public GameObject sheetPlacement;
    public LayerMask ARKitGroundLayer;

    private GameObject tableObject;
    private GameObject placeButton;
    private bool isPlaced;
    private InstructionSteps instructionSteps;

	// Use this for initialization
	void Start () {
        placeButton = GetComponentInChildren<SpriteRenderer>().gameObject;
        instructionSteps = FindObjectOfType<InstructionSteps>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPlaced)
        {
            RaycastHit hit;
            if (Physics.Raycast(BevelInput.GetCameraRay(), out hit, 10f, ARKitGroundLayer))
            {
                transform.position = hit.point;
            }
        }

	}


    public void PlaceTable(){
        isPlaced = true;
        placeButton.SetActive(false);
        tableObject = Instantiate(tablePrefab, transform.position, Quaternion.identity, transform);
        instructionSteps.enabled = true;
    }

}
