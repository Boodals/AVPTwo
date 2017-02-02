using UnityEngine;
using System.Collections;

public class ChangeSceneButton : MonoBehaviour
{

	public int sceneId;
	new public Camera camera;

	private bool ignoreLooking = false;

	void Update()
	{
		Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;

		if(GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
		{

			if(!ignoreLooking)
			{
				//Looked at, show the window
				SceneMgr.ChangeWindow(sceneId);

				if(Input.GetMouseButtonDown(0))
				{
					//Clicked, change the scene

					//Debug.Log("Clicked " + gameObject.name);
					SceneMgr.ChangeScene(sceneId);

					ignoreLooking = true;
				}
			}
		}
		else
		{
			//We looked off of it, enable viewing again
			ignoreLooking = false;
		}
	}

}
