using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class MarkUp : MonoBehaviour {

    public string title;
    public GameObject markedObject;
    public Vector3 relativePosition;
    public DateTime dateCreated;
    public string testResult;

    public bool isSolved;

    public enum MarkUpType {question, issue, testResult}
    public MarkUpType markUpType;

    public TextMesh dataDisplay;



    public void DisplayInfo()
    {
        dataDisplay.text = "Markup Type: " + markUpType.ToString() + "\n";
        dataDisplay.text += string.Format("Marked Object: {0} \n", markedObject.ToString());
        dataDisplay.text += "Level: 1 \n";
        dataDisplay.text += "Room: Captain Marvel \n";
        dataDisplay.text += "Project Coordinates: " + relativePosition.ToString() +"\n";
        dataDisplay.text += "Date Created: " + dateCreated +"\n";
        dataDisplay.text += "Created by: Logan Smith \n";

        if (markUpType == MarkUpType.testResult)
        {
            dataDisplay.text += "Test Result: " + testResult +"\n";

        }


        dataDisplay.gameObject.SetActive(true);        
        
    }

    public void SetTestResult(string result)
    {
        testResult = result;
    }

    public void SetType(int typeInput)
    {
        markUpType = (MarkUpType)Enum.ToObject(typeof(MarkUpType), typeInput);
    }

    // Use this for initialization
    void Start () {
        dataDisplay.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
