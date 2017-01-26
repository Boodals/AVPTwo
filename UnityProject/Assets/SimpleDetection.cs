using UnityEngine;
using System.Collections;
using System;

public class SimpleDetection : DetectonBase {

    public CuttofScroller cOS;
    public bool testingBool;
    public int returnVal;
    public override void act()
    {
        cOS.beginTransition(testingBool);
    }
    public override int retrunVal()
    {
        return returnVal;
    }
}
