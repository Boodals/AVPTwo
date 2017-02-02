using UnityEngine;
using System.Collections;

public class CityColourChange : MonoBehaviour {


    public Material InMatOne;
    public Material InMatTwo;
    private Material currentMat;
    float blendVal;
    private GameObject[] cityObjects;
    private Color baseColur;

    bool flip;
	// Use this for initialization
	void Start ()
    {
        baseColur = Color.gray;
        currentMat = Instantiate(InMatOne);
        cityObjects = GameObject.FindGameObjectsWithTag("CITYBUILDING");
        foreach(GameObject go in cityObjects)
        {
            go.GetComponent<Renderer>().material = InMatOne;
        }

        flip = true;

	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKey(KeyCode.J))
        {

            baseColur = Color.Lerp(InMatOne.color, InMatTwo.color, Mathf.PingPong(Time.time, 1));

                if(InMatOne == InMatTwo)
            {
                Material HOLD;
                HOLD = InMatOne;

                InMatOne = InMatTwo;
                InMatTwo = HOLD;


            }
        }

        foreach (GameObject go in cityObjects)
        {
            go.GetComponent<Renderer>().material.SetColor("_Color", baseColur);
        }



    }
}
