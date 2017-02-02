using UnityEngine;
using System.Collections;

public class ChangeSceneButton : MonoBehaviour
{

	public string sceneName;
	new public Camera camera;
	
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			RaycastHit hit;

			if(GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
			{
				//Do change scene stuff

				Debug.Log("Clicked " + gameObject.name);
				SceneMgr.ChangeScene(sceneName);
			}
		}
	}
	
}
