using UnityEngine;
using System.Collections;

public class CuttofScroller : MonoBehaviour {

	public delegate void CutOffScrollerEvent(CuttofScroller cutOff);

    public float speed;
    Material alphaBlender;
    //private float currentBlend;
    public bool direcetion;
    private bool transition;

	public event CutOffScrollerEvent OnFinished;
	
	// Use this for initialization
	void Start () {
        alphaBlender = GetComponent<Renderer>().material;
        //currentBlend = 0;
        direcetion = true;
        transition = false;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (transition)
        {
            if (direcetion)
            {
                if (alphaBlender.GetFloat("_Cutoff") < 1)
                {
                    alphaBlender.SetFloat("_Cutoff", alphaBlender.GetFloat("_Cutoff") + speed);
                }
                else
                {
                    transition = false;
                    pingControler();
                }

            }
            else
            {
                if (alphaBlender.GetFloat("_Cutoff") > 0)
                {
                    alphaBlender.SetFloat("_Cutoff", alphaBlender.GetFloat("_Cutoff") - speed);
                }
                else
                {
                    transition = false;
                    pingControler();
                }
            }
        }
        
	}

    //Call me to being chaning the screen transparncy
    public void beginTransition(bool _direction)
    {
			transition = true;
            direcetion = _direction;
        
    }

    public void forceTransition(bool _in)
    {
        if(_in)
        {
            GetComponent<Renderer>().material.SetFloat("_Cutoff", 1);
        }
        else
        {
            GetComponent<Renderer>().material.SetFloat("_Cutoff", 0);
        }


    }

    private void pingControler()
    {
		if(OnFinished != null)
		{
			OnFinished(this);
        }
    }
}

