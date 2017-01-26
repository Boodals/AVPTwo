using UnityEngine;
using System.Collections;

public class meshChanger : MonoBehaviour {

    public GameObject CityMesh;
    public GameObject SkyMesh;
    public GameObject MoterWayMesh;

    // Use this for initialization
    void Start () {
        GetComponent<MeshFilter>().mesh = CityMesh.GetComponent<MeshFilter>().mesh;
        gameObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);


    }
	
	// Update is called once per frame
	public void swapMesh (int _in)
    {

        switch (_in)
        {
            case 1:
                gameObject.transform.localScale = new Vector3(0.02f, 0.020f, 0.02f);
             
                GetComponent<MeshFilter>().mesh = SkyMesh.GetComponent<MeshFilter>().mesh;

                break;

            case 2:
                gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                GetComponent<MeshFilter>().mesh = MoterWayMesh.GetComponent<MeshFilter>().mesh;

                break;

            case 3:
                gameObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                GetComponent<MeshFilter>().mesh = CityMesh.GetComponent<MeshFilter>().mesh;

                break;
        }
	}
}
