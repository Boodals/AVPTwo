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


	public MeshChanger holoMesh;

	public int currentSceneID = 0;
	private int screenSceneID;
	private bool isChangingWindow = false;
	private bool isChangingScene = false;
	private int sceneIDToChangeTo;
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
		if(isChangingScene || isChangingWindow)
		{
			return;
		}

		//Debug.Log(screenSceneID + " != " + sceneId);

		if(screenSceneID != sceneId)
		{
			//Change the screen to show the scene
			isChangingWindow = true;

			screenSceneID = sceneId;

			if(windowPictures[sceneId] != null)
			{
				windowPictures[sceneId].OnFinished += OnWindowFinished;
            }
			else if(windowPictures[currentSceneID] != null)
			{
				windowPictures[sceneId].OnFinished += OnWindowFinished;
			}
			else
			{
				isChangingWindow = false; //No animation is playing, dont need to wait (also something probably broke)
			}

			SetActiveWindow(sceneId);
			
			holoMesh.swapMesh(sceneId);

			return;
		}

		isChangingScene = true;
		followPath.enabled = true;
		followPath.speed = pathSpeed;
		sceneIDToChangeTo = sceneId;

		//Debug.Log("Moving to scene " + sceneName);

		followPath.OnEndOfPath += OnGetToDoor;

	}

	private void OnWindowFinished(CuttofScroller cutOff)
	{
		cutOff.OnFinished -= OnWindowFinished;

		isChangingWindow = false;
	}

	private void OnGetToDoor()
	{
		SceneManager.UnloadScene(sceneNames[currentSceneID]);
		
		//Debug.Log("Loading scene " + sceneToChangeTo);
		loadAsync = SceneManager.LoadSceneAsync(sceneNames[sceneIDToChangeTo], LoadSceneMode.Additive);

		followPath.enabled = false;

		//Stop listening here
		followPath.OnEndOfPath -= OnGetToDoor;
	}

	private void LoadedScene()
	{
		//Debug.Log("Done loading");
		currentSceneID = sceneIDToChangeTo;
		isChangingScene = false;

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


	private void SetActiveWindow(int ID, bool doForce = false)
	{
		int i = 0;
		foreach(CuttofScroller cutOff in windowPictures)
		{
			if(cutOff != null)
			{
				if(doForce)
				{
					//Debug.Log(doForce + " forceTransition(" + ID + " != " + i + " = " + (ID != i) + ")");
					cutOff.forceTransition(ID != i);
				}
				else
				{
					//Debug.Log(doForce + " beginTransition(" + ID + " != " + i + " = " + (ID != i) + ")");
					cutOff.beginTransition(ID != i);
				}
			}
			i++;
		}
	}
}
