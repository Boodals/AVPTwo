using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FollowPath))]
public class FollowPathController : MonoBehaviour
{

	public float speedChangeSensitivity = 0.5f;

	private FollowPath followPath;

	private float speed;

	void Awake()
	{
		followPath = GetComponent<FollowPath>();
		speed = followPath.speed;
	}

	void Update()
	{
		speed += Input.GetAxis("Mouse ScrollWheel") * speedChangeSensitivity;
		followPath.speed = Input.GetAxis("Vertical") * speed;
	}
	
}
