using UnityEngine;
using System.Collections;

public class FreeCamera : MonoBehaviour
{

	public float moveSpeed = 0.5f;
	
	public float momentumScale = 0.5f;
	public float momentumFalloff = 50f;

	private Vector3 momentum;

	void Update()
	{
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("ActuallyVertical"), Input.GetAxis("Vertical")) * moveSpeed;

        Quaternion yaw = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

		transform.position += (yaw * movement + momentum) * Time.deltaTime;


		momentum += (yaw * movement) * momentumScale;

		momentum *= momentumFalloff * Time.deltaTime;
	}
	
}
