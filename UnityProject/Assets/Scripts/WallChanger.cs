using UnityEngine;
using System.Collections;

public class WallChanger : MonoBehaviour {

    public float speed;
    public CuttofScroller wallOne;
    public CuttofScroller wallTwo;


    void Awake()
    {
        wallOne.forceTransition(true);
        wallTwo.forceTransition(true);
    }

    // Use this for initialization
    public void blendWall(int _in)
    {
        switch(_in)
        {
            case 1:
                wallOne.beginTransition(false);
                wallTwo.beginTransition(true);
                break;
            case 2:
                wallOne.beginTransition(true);
                wallTwo.beginTransition(false);
                break;

            case 3:
                wallOne.beginTransition(true);
                wallTwo.beginTransition(true);
                break;
        }


    }
	

}
