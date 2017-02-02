using UnityEngine;
using System.Collections;

public class FreeRotation : MonoBehaviour
{

	public float lookSpeed = 2f;

	public float momentumScale = 40f;
	public float momentumFalloff = 50f;

	public bool doLockMouse = true;

	private Vector2 yawPitch;
	private Vector2 yawPitchMomentum = new Vector2();

	void Awake()
	{
		yawPitch = new Vector2(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x);
		
		if(doLockMouse)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void Update()
	{

		if(Cursor.visible)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				return;
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}


		float yaw = Input.GetAxis("Mouse X") * lookSpeed;
		float pitch = -Input.GetAxis("Mouse Y") * lookSpeed;
		//Yaw
		yawPitch.x = Mathf.Repeat(yawPitch.x + yaw + yawPitchMomentum.x * Time.deltaTime, 360f);

		//Pitch
		yawPitch.y = Mathf.Clamp(yawPitch.y + pitch + yawPitchMomentum.y * Time.deltaTime, -90f, 90f);


		yawPitchMomentum += new Vector2(yaw, pitch) * momentumScale;

		yawPitchMomentum *= momentumFalloff * Time.deltaTime;
    }

	void LateUpdate()
	{
		transform.rotation = Quaternion.Euler(yawPitch.y, yawPitch.x, 0f);
	}

}
