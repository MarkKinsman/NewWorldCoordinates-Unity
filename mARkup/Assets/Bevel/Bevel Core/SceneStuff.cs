using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStuff : MonoBehaviour {

    public GameObject[] activateOnAwake;

    //[Tooltip("If Debug Mode is enabled, the scene will not restart on focus.")]
    //public bool debugMode = false;

    private void Awake()
    {
        foreach (var item in activateOnAwake)
        {
            item.SetActive(true);
        }
    }

    // Use this for initialization
    void Start () {
		Application.runInBackground = true;
	}

	// Update is called once per frame
	void Update () {
	}

	void OnApplicationFocus(){
        if (!Application.isEditor)
        {
            ResetScene();
        }
	}

	public void ResetScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

    //For stepping to the next scene, flexible to number of scenes
    public void StepScene(){
        Scene scene = SceneManager.GetActiveScene();

        if (scene.buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SwitchScene(scene.buildIndex + 1);
        }
        else{
            SwitchScene(0);
        }
    }

	//For replacing the current scene(s) with a single new scene
	public void SwitchScene(string sceneName){
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}
	public void SwitchScene(int sceneIndex){
        Debug.Log("sceneCount: " + SceneManager.sceneCountInBuildSettings.ToString());
        Debug.Log("switching form scene " +  SceneManager.GetActiveScene().buildIndex.ToString() +
                  " to scene " + sceneIndex.ToString());
		SceneManager.LoadScene (sceneIndex, LoadSceneMode.Single);
	}
	//For layering additional scenes onto the current scene
	public void AddScene(string sceneName){
		SceneManager.LoadScene (sceneName, LoadSceneMode.Additive);
	}
	public void AddScene(int sceneIndex){
		SceneManager.LoadScene (sceneIndex, LoadSceneMode.Additive);
	}



}
