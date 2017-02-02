using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour
{
	public enum RepeatMode
	{
		None,
		Repeat,
		PingPong,
	}

	public delegate void PathEventHandler();

	public Path path;

	public bool lockRotation = false;

	public float speed = 3f;


	public float distOnPath = 0f;

	public RepeatMode repeatMode = RepeatMode.None;

	private bool pingPongDirInv = false;

	/// <summary>
	/// Called every LateUpdate the progress along path is 0
	/// </summary>
	public event PathEventHandler OnStartOfPath;

	/// <summary>
	/// Called every LateUpdate the progress along path is the paths length
	/// </summary>
	public event PathEventHandler OnEndOfPath;



	void OnValidate()
	{
		UpdateObject();

		if(path != null)
		{
			distOnPath = Mathf.Clamp(distOnPath, 0f, path.length);
		}
	}

	void LateUpdate()
	{
		if(distOnPath <= 0f && OnStartOfPath != null)
		{
			OnStartOfPath();
		}

		distOnPath += speed * Time.deltaTime;

		if(distOnPath >= path.length && OnEndOfPath != null)
		{
			OnEndOfPath();
		}

		switch(repeatMode)
		{
			case RepeatMode.None:
				distOnPath = Mathf.Clamp(distOnPath, 0f, path.length);
				UpdateObject();
				break;
			case RepeatMode.Repeat:
				distOnPath = Mathf.Repeat(distOnPath, path.length);
				UpdateObject();
				break;
			case RepeatMode.PingPong:
				if(!pingPongDirInv)
				{
					distOnPath = Mathf.Repeat(distOnPath, path.length * 2f);
					UpdateObject(Mathf.PingPong(distOnPath, path.length));
				}
				else
				{
					distOnPath = path.length - Mathf.Clamp(path.length - distOnPath, 0f, path.length);
				}
				break;
		}
	}

	public void UpdateObject()
	{
		UpdateObject(distOnPath);
	}
	public void UpdateObject(float dist)
	{
		if(path != null && isActiveAndEnabled)
		{
			transform.position = path.GetPos(dist);

			if(lockRotation)
			{
				transform.rotation = Quaternion.LookRotation(path.GetNormal(distOnPath));
			}
		}
	}
}
