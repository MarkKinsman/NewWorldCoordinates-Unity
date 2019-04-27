using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseSwitch : MonoBehaviour {

    public enum Phase { Existing, New};
    public Phase activePhase = Phase.Existing;
    public GameObject existingContent;
    public GameObject newContent;
    

    public Text buttonText;

	// Use this for initialization
	void Start () {
        PhaseToExisting();
	}
	
   
    public void SwitchPhase()
    {
        if (activePhase == Phase.Existing)
	    {
            PhaseToNew();
	    }
        else
        {
            PhaseToExisting();
        }
    }
    public void PhaseToNew()
    {
        activePhase = Phase.New;
        buttonText.text = "New";
        existingContent.SetActive(false);
        newContent.SetActive(true);
    }
    public void PhaseToExisting()
    {
        activePhase = Phase.Existing;
        buttonText.text = "Existing";
        existingContent.SetActive(true);
        newContent.SetActive(false);
    }

}
