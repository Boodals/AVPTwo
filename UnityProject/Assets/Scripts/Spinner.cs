using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {
    int speed;
	// Use this for initialization
	void Start () {
        if (Random.Range(0, 5) >= 1)
        {
            speed = Random.Range(10, 100);
        }
        else
        {
            speed = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
