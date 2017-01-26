using UnityEngine;
using System.Collections;

public class TransitionControler : MonoBehaviour {

    //The screen that will hide the transition
    CuttofScroller m_cOS;
    customRayCaster m_customRay; 

    //The nodes that will trigger the transitons 
    public GameObject[] m_roomTransitionPoints;
    private bool readyToload;
    public bool isWindowClosed;
    public meshChanger MC;

    void Awake()
    {
        readyToload = true;
        m_cOS = GameObject.Find("ControlRoomWindow").GetComponent<CuttofScroller>();
        m_customRay = Camera.main.GetComponent<customRayCaster>();
        isWindowClosed = true;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (readyToload)
            {
                //If we have found somthing
                GameObject Go = m_customRay.CastRay();
                if (Go != null)
                {
                    if (Go.GetComponent<DetectonBase>() != null)
                    {
                        transitioning();
                        int hold = Go.GetComponent<DetectonBase>().retrunVal();
                        GetComponent<WallChanger>().blendWall(hold);
                        MC.swapMesh(hold);
                    }
                }
            }
        }

    }

    public void transitioning()
    {
        readyToload = false;
    }

    public void informReady()
    {
        readyToload = true;
        isWindowClosed = !isWindowClosed;
    }

    public bool getOpenState()
    {
        return isWindowClosed;
    }

}
