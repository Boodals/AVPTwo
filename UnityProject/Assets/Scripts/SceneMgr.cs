using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
	private static SceneMgr singleton;

	public static void ChangeScene(string sceneName)
	{
		singleton.StartChangeScene(sceneName);
	}


	public string currentScene;
	private bool isChangingScene = false;
	private string sceneToChangeTo;
	public FollowPath followPath;


	private AsyncOperation loadAsync;

	private float pathSpeed;


	void Awake()
	{
		if(singleton != null)
		{
			//Debug.LogError("Multiple SceneMgrs");
			Destroy(this);
			return;
		}

		singleton = this;


		pathSpeed = followPath.speed;
    }


	void Update()
	{
		if(isChangingScene && loadAsync != null && loadAsync.isDone)
		{
			LoadedScene();
			loadAsync = null;
        }
	}

	private void StartChangeScene(string sceneName)
	{
		//Disable all the buttons
		foreach(ChangeSceneButton button in FindObjectsOfType<ChangeSceneButton>())
		{
			button.enabled = false;
		}

		isChangingScene = true;
		followPath.enabled = true;
		followPath.speed = pathSpeed;
		sceneToChangeTo = sceneName;

		//Debug.Log("Moving to scene " + sceneName);

		followPath.OnEndOfPath += OnGetToDoor;

	}

	private void OnGetToDoor()
	{
		if(currentScene != "")
		{
			bool isUnloaded = SceneManager.UnloadScene(currentScene);
			//Debug.Log("Unloaded: " + isUnloaded);
		}
		//Debug.Log("Loading scene " + sceneToChangeTo);
		loadAsync = SceneManager.LoadSceneAsync(sceneToChangeTo, LoadSceneMode.Additive);

		followPath.enabled = false;

		//Stop listening here
		followPath.OnEndOfPath -= OnGetToDoor;
	}

	private void LoadedScene()
	{
		//Debug.Log("Done loading");
		currentScene = sceneToChangeTo;

		followPath.enabled = true;
		followPath.speed = -followPath.speed;

		//Listen on the next step
		followPath.OnStartOfPath += OnGetBackFromDoor;
	}

	private void OnGetBackFromDoor()
	{
		//Debug.Log("Back from door");

		followPath.enabled = false;

		//Reenable all the buttons
		foreach(ChangeSceneButton button in FindObjectsOfType<ChangeSceneButton>())
		{
			button.enabled = true;
		}
		
		followPath.OnStartOfPath -= OnGetBackFromDoor;
	}

}
