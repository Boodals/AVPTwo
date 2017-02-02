using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
	private static SceneMgr singleton;

	public static void ChangeScene(int sceneId)
	{
		singleton.StartChangeScene(sceneId);
	}
	public static void ChangeWindow(int sceneId)
	{
		singleton.SetActiveWindow(sceneId);
	}


	public MeshChanger holoMesh;

	public int currentSceneID = 0;
	private int screenSceneID;
	private int changingToSceneID;
	private bool isChangingScene = false;
	private bool isChangingWindow = false;
    private bool isMoving = false;
	public FollowPath followPath;


	public string[] sceneNames =
	{
		"City",
		"Motorway",
		"FloatingIslands"
	};

	public CuttofScroller[] windowPictures;


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

		SetActiveWindow(currentSceneID, true);
	}

	void Update()
	{
		if(isChangingScene && loadAsync != null && loadAsync.isDone)
		{
			LoadedScene();
			loadAsync = null;
        }
	}

	private void StartChangeScene(int sceneId)
	{
		//Debug.Log("StartChangeScene " + sceneId + ": " + sceneNames[sceneId] + ", " + isChangingScene + " || " + isMoving + " || " + isChangingWindow);
		if(isChangingScene || isMoving || isChangingWindow)
		{
			return;
		}

		//Debug.Log(screenSceneID + " != " + sceneId);

		//Debug.Log("Moving to scene " + sceneName);

		changingToSceneID = sceneId;
		
		if(screenSceneID != currentSceneID && windowPictures[screenSceneID] == null)
		{
			//Close the window first, so they cant see things loading

			//Debug.Log("Closing window first");

			SetActiveWindow(currentSceneID);
			isChangingScene = true;
		}
		else
		{
			isChangingScene = true;
			SetScene(sceneId);
		}

	}
	
	private void LoadedScene()
	{
		//Debug.Log("Done loading");
		isChangingScene = false;

		//Hide the window
		SetActiveWindow(0);
	}


	private void StartMoveToDoor()
	{
		if(isChangingScene || isMoving || isChangingWindow)
		{
			return;
		}

		followPath.enabled = true;
		followPath.speed = pathSpeed;

		followPath.OnEndOfPath += OnGetToDoor;
	}

	private void OnGetToDoor()
	{
		followPath.enabled = false;

		//Stop listening here
		followPath.OnEndOfPath -= OnGetToDoor;



		//When we escape out of the free camera or whatever:

		followPath.enabled = true;
		followPath.speed = -followPath.speed;

		//Listen on the next step
		followPath.OnStartOfPath += OnGetBackFromDoor;
	}

	private void OnGetBackFromDoor()
	{
		//Debug.Log("Back from door");

		followPath.enabled = false;
		
		followPath.OnStartOfPath -= OnGetBackFromDoor;
	}


	private void SetActiveWindow(int sceneId, bool doForce = false)
	{
		if(screenSceneID == sceneId && !doForce)
		{
			//Same screen
			return;
		}

		//Debug.Log("SetActiveWindow " + sceneId + ": " + sceneNames[sceneId] + ", " + isChangingScene + " || " + isMoving + " || " + isChangingWindow);
		if(isChangingScene || isMoving || isChangingWindow)
		{
			return;
		}

		screenSceneID = sceneId;

		holoMesh.swapMesh(sceneId);

		if(windowPictures[sceneId] != null)
		{
			isChangingWindow = true;
			windowPictures[sceneId].OnFinished += OnWindowFinish;
		}
		else if(windowPictures[currentSceneID] != null)
		{
			isChangingWindow = true;
			windowPictures[currentSceneID].OnFinished += OnWindowFinish;
        }
		else
		{
			//Changing from null window to null window

			//Do nothing
		}


		int i = 0;
		foreach(CuttofScroller cutOff in windowPictures)
		{
			if(cutOff != null)
			{
				if(doForce)
				{
					//Debug.Log(doForce + " forceTransition(" + ID + " != " + i + " = " + (ID != i) + ")");
					cutOff.forceTransition(sceneId != i);
				}
				else
				{
					//Debug.Log(doForce + " beginTransition(" + ID + " != " + i + " = " + (ID != i) + ")");
					cutOff.beginTransition(sceneId != i);
				}
			}
			i++;
		}
	}

	private void OnWindowFinish(CuttofScroller cutOff)
	{
		//Debug.Log("OnWindowFinish " + isChangingScene);


		isChangingWindow = false;

		//Stop listening
		if(cutOff != null)
		{
			cutOff.OnFinished -= OnWindowFinish;
		}


		if(isChangingScene)
		{
			SetScene(changingToSceneID);
		}
		else if(currentSceneID != 0 && windowPictures[screenSceneID] != null)
		{
			//They arent looking, unload the scene!

			//Debug.Log("They arent looking, unload the scene! " + screenSceneID + " != " + currentSceneID + " && " + (windowPictures[screenSceneID] != null));
			
			SetScene(0);
		}
	}

	private void SetScene(int sceneId)
	{
		//Debug.Log("SetScene " + sceneId + ", " + sceneNames[sceneId]);


		SceneManager.UnloadScene(sceneNames[currentSceneID]);

		//Debug.Log("Loading scene " + sceneToChangeTo);
		

		loadAsync = SceneManager.LoadSceneAsync(sceneNames[sceneId], LoadSceneMode.Additive);
		
		currentSceneID = sceneId;

		if(loadAsync == null)
		{
			//Failed to load, scene probably doesnt exist
			Debug.LogWarning("Failed to load scene");

			LoadedScene();
		}
	}

}
