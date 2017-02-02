using UnityEngine;
using System.Collections;

public class MeshChanger : MonoBehaviour {

	public Mesh[] meshPerScene;
	public Vector3[] meshScale;

    // Use this for initialization
    void Start () {
		GetComponent<MeshFilter>().mesh = meshPerScene[0];
		gameObject.transform.localScale = meshScale[0];
    }
	
	// Update is called once per frame
	public void swapMesh (int _in)
    {
		GetComponent<MeshFilter>().mesh = meshPerScene[_in];
	}
}

