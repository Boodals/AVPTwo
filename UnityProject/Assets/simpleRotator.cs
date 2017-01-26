using UnityEngine;
using System.Collections;

public class simpleRotator : MonoBehaviour {

    public enum Rot
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        FORWARD,
        BACKWARD
    }
    public Rot rotationDirection;

    public float speed;
    public bool directon;

	// Update is called once per frame
	void Update () {
        Vector3 dir = new Vector3();

        switch(rotationDirection)
        {
            case Rot.UP:
                dir = Vector3.up;
                break;

            case Rot.DOWN:
                dir = Vector3.down;
                break;

            case Rot.LEFT:
                dir = Vector3.left;
                break;

            case Rot.RIGHT:
                dir = Vector3.right;
                break;

            case Rot.FORWARD:
                dir = Vector3.forward;
                break;

            case Rot.BACKWARD:
                dir = Vector3.back;
                break;                
        }
        if (directon)
        {
            transform.Rotate(dir * (Time.deltaTime) * speed);
        }
        else
        {
            transform.Rotate(dir * (Time.deltaTime) * (speed * -1));
        }
	}
}
