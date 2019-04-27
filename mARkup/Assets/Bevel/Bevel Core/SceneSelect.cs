using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour {

	public Text debugText;

	public Dropdown sceneDropdown;
	public string[] sceneNames;

	// Use this for initialization
	void Start () {

		sceneNames = new string[SceneManager.sceneCountInBuildSettings];

		//create an array of scenes for setting up the dropdown
		for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
			
			SceneManager.LoadSceneAsync (i, LoadSceneMode.Additive); //testing getting real scene names by loading then unloading

			sceneDropdown.options.Add (new Dropdown.OptionData (SceneManager.GetSceneByBuildIndex (i).name));
			sceneNames [i] = SceneManager.GetSceneByBuildIndex (i).name; //probably just for testing purposes

			//SceneManager.UnloadSceneAsync (SceneManager.GetSceneByBuildIndex (i));
		}
		StartCoroutine (WaitAndUnloadAll ());
	}

	//coroutine to wait and unload all the other scenes
	IEnumerator WaitAndUnloadAll(){
		//yield return new WaitForSeconds (.5f);
		while (!CheckSceneLoad()) {
			yield return new WaitForSeconds(.5f);
			Debug.Log ("waiting for unload");
			debugText.text = "Waiting for unload";
		}

		UnloadAll ();
	}
	public void UnloadAll(){
		Debug.Log("Unloading scenes");
		debugText.text = "Unloading scenes...";
		for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
			if (SceneManager.GetSceneByBuildIndex (i).isLoaded) {
				SceneManager.UnloadSceneAsync (i);
			}
		}
	}

	//test the choosing of scenes by just applying the name to a label
	public void LabelSceneName(){
		Dropdown.OptionData[] optionArray = sceneDropdown.options.ToArray();
		debugText.text = optionArray[sceneDropdown.value].text;
	}

	public void loadSceneOnTop(){
		UnloadAll();
		StartCoroutine (SceneLoader ());
	}

	void Update(){
	}

	IEnumerator SceneLoader(){
		//my custom wait until because I don't understand func
		while (SceneManager.sceneCount > 1) {
			yield return new WaitForSeconds(.1f);
		}

		SceneManager.LoadSceneAsync (sceneDropdown.value + 1, LoadSceneMode.Additive);

	}

	//checks if all the extrascenes are unloaded. Probably uneccessary by sceneCount
	bool CheckSceneUnload(){
		for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
			if (SceneManager.GetSceneByBuildIndex (i).isLoaded) {
				return false;
			}
		}
		return true;
	}
	//checks if all the extrascenes are unloaded. Probably uneccessary by sceneCount
	bool CheckSceneLoad(){
		for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
			Debug.Log ("Checking scene load...");
			if (!SceneManager.GetSceneByBuildIndex (i).isLoaded) {
				Debug.Log ("Scene " + i.ToString () + " is not loaded. Returning false.");
				return false;
			}
		}
		Debug.Log ("All scenes are loaded. Returning True");
		return true;
	}
}
