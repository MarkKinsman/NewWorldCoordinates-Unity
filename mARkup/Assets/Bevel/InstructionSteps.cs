using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionSteps : MonoBehaviour {
    
    [Tooltip("Drag in a piece of UI Text to display instructions.")]
    public Text instructionText;
    [Tooltip("Drag in a piece of UI Text to display the step number.")]
    public Text stepLabel;
	[Tooltip("When it reaches the end, does it stop or circle around.")]
	public bool cycleSteps = false;

	 GameObject[] StepObjects;
	 Transform[] steps;
	public List<Transform> stepList; //why does this need to be public??
	 string[] stepNames;
    int currentStep = 0;
    ActivityLog activityLog;

	// Use this for initialization
	void Start () {
        activityLog = ActivityLog.Setup();
		//stepNames = new string[9]{"Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"} ;
		foreach (Transform child in transform) {
			stepList.Add(child);
		}
		steps = stepList.ToArray ();
		StepObjects = new GameObject[steps.Length];
		stepNames = new string[steps.Length];
		for (int i = 0; i < steps.Length; i++) {
			StepObjects [i] = steps [i].gameObject;
			stepNames [i] = StepObjects [i].name;
		}

        instructionText.text = "";
        ShowStep(currentStep);


    }

    public void StepForward()
    {
        if (currentStep < StepObjects.Length - 1)
        {
            currentStep++;
            ShowStep(currentStep);
            instructionText.text = "";
        }
		else if (cycleSteps) {
			currentStep = 0;
			ShowStep(currentStep);
			instructionText.text = "";
		}
        else
        {
            instructionText.text = "That's the end. Try stepping backwards.";
        }

    }

    public void StepBackward()
    {
        if (currentStep > 0)
        {
            currentStep--;
            ShowStep(currentStep);
            instructionText.text = "";
        }
		else if (cycleSteps) {
			currentStep = steps.Length-1;
			ShowStep(currentStep);
			instructionText.text = "";
		}
        else
        {
            instructionText.text = "That's the beginning. Try stepping forward.";
        }
        
    }

    public void ShowStep(int stepNumber)
    {
        try
        {
            for (int i = 0; i < StepObjects.Length; i++)
            {
                StepObjects[i].SetActive(false);
            }
            StepObjects[stepNumber].SetActive(true);
            //stepLabel.text = string.Format("Object {0}", stepNumber + 1);
			stepLabel.text = stepNames[stepNumber];
        }
        catch (System.IndexOutOfRangeException e)
        {
            activityLog.LogActivity(string.Format("Oh shit! Out of range. Step number {0} is outside of size {1}. System Expection: {2}", stepNumber, StepObjects.Length, e));
        }
    }

}
