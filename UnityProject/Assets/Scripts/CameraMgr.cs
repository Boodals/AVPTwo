using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour
{

	void Update()
	{
		if(Input.GetButtonDown("Space"))
		{
			bool on = GetComponent<FollowPathController>().enabled;
			GetComponent<FollowPathController>().enabled = !on;
			GetComponent<FollowPath>().enabled = !on;
			GetComponent<FreeCamera>().enabled = on;
		}
	}

}
