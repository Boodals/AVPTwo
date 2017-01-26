using UnityEngine;
using System.Collections;

public class customRayCaster : MonoBehaviour {
    public bool switcher;
    // Update is called once per frame


    public GameObject CastRay() {

        Vector3 forward = transform.transform.TransformDirection(Vector3.forward);
        RaycastHit rayOut;

        Debug.DrawRay(transform.position, forward);

        if(Physics.Raycast(transform.position, forward,out rayOut))
        {

            print("HIT"+rayOut.transform.gameObject.name);
            if(rayOut.transform.gameObject.GetComponent<SimpleDetection>() != null)
            {
                return rayOut.transform.gameObject;
            }
        }
        return null;
    }
}
