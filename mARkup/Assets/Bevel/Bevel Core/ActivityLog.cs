using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


public class ActivityLog : MonoBehaviour {

	string defaultFilePath;

	public static ActivityLog Setup ()
	{
		ActivityLog activityLog = GameObject.FindObjectOfType<ActivityLog> ();
		if (!activityLog) {
			new GameObject ("Activity Log").AddComponent<ActivityLog>();
			activityLog = GameObject.FindObjectOfType<ActivityLog> ();
		}
		return activityLog;
	}

	// Use this for initialization
	void Awake () {
		
		defaultFilePath = Application.persistentDataPath + @"/Saves/" + SceneManager.GetActiveScene ().name + "-log";
		//File.Open ("poop",FileMode.Open);
		LogActivity("New Log - " + System.DateTime.Now.Date.ToString() + " " + System.DateTime.Now.TimeOfDay.ToString());
	}

	//function that writes string as a new line to the log file
	public void LogActivity(string activity){
		LogActivity (activity, defaultFilePath);
	}
		
	//overload that writes array of strings, each as new line to the log file
	public void LogActivity(string[] activities){
		LogActivity (activities, defaultFilePath);
	}
	//overload with single string and a custom filepath
	public void LogActivity(string activity, string filePath){
		string[] activities = new string[1];
		activities [0] = activity;
		LogActivity (activities, filePath);
	}
	//overload with string array and custom filepath. 
	public void LogActivity(string[] activities, string filePath){
		try {

			for (int i = 0; i < activities.Length; i++) {
				Debug.Log("LOG - " + activities[i]);
				File.AppendAllText(filePath, activities[i] + System.Environment.NewLine);
			}
			//File.WriteAllLines(filePath,activities);
			//FileStream fileStream = File.OpenWrite(filePath);
			//fileStream.Write(

		} catch (FileNotFoundException e) {
			File.Create (filePath);
			LogActivity (activities, filePath);
            Debug.LogWarning("Trying to log activity, Exception: " + e.ToString());
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
