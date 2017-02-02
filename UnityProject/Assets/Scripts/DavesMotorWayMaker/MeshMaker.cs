using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshMaker : MonoBehaviour
{

    //public List<GameObject> InitalShape;
    public GameObject pointPrefab;

    private SplineDecorator decor;



    private List<GameObject> Inputpoints;
    private Vector3[] pointLocs;
    //private Vector2[] UVs;
    private int[] tris;
    private Mesh model;
    public int numOfIterations;
    public Material _inMat;
    private int nodesPerSegment;
    private GameObject MeshHolder;
    private int counter = 0;
    public bool capped;
   // VertexColour vCol;

    private SplineDecorator spD;

    public void Begin()
    {
       // vCol = GetComponent<VertexColour>();
        model = new Mesh();
        MeshHolder = new GameObject();
        MeshHolder.AddComponent<MeshFilter>();
        MeshHolder.GetComponent<MeshFilter>().mesh = model;
        model.name = "wingMesh";
        if (MeshHolder.GetComponent<MeshRenderer>() == null)
        {
            MeshHolder.gameObject.AddComponent<MeshRenderer>();
        }
    }

    public Mesh getModel()
    {
        return model;
    }



    public void createMesh()
    {
        if(model == null)
        {
            model = new Mesh();
        }


        spD = GetComponent<SplineDecorator>();
        counter = 0;
        Inputpoints = spD.getPointList();
        nodesPerSegment = Inputpoints.Count / spD.frequency;
        if (capped)
        {
            tris = new int[((((nodesPerSegment * 2) * 3)) * (spD.frequency - 1)) + (((nodesPerSegment - 2) * 3) * 2)];
        }
        else
        {
            tris = new int[((((nodesPerSegment * 2) * 3)) * (spD.frequency - 1))];
        }

        pointLocs = new Vector3[Inputpoints.Count];
        foreach (GameObject go in Inputpoints)
        {
            pointLocs[counter++] = go.transform.position;
        }

        counter = 0;
        for (int i = 0; i < spD.frequency - 1; i++)
        {
            calcTris(nodesPerSegment * i);
        }
        if (capped)
        {
            calcEndCaps();
        }
        model.vertices = pointLocs;
        //model.uv = UVs;
        model.triangles = tris;
        MeshHolder.gameObject.GetComponent<Renderer>().material = _inMat;
        model.RecalculateNormals();

      //  vCol.MakeColour(model);
    }

    private void calcTris(int _offset)
    {

        //counter = 0;
        int numOfFacesToCalc = nodesPerSegment;
        int startPointA = 0 + _offset;
        int startPointB = numOfFacesToCalc + _offset;
        int startPointC = (startPointB + 1);
        //calc all faces but the wraparound
        for (int i = 0; i < numOfFacesToCalc - 1; i++)
        {
            tris[counter] = startPointA;
            tris[counter + 1] = startPointB;
            tris[counter + 2] = startPointC;

            tris[counter + 3] = startPointA;
            tris[counter + 4] = startPointB + 1;
            tris[counter + 5] = startPointA + 1;

            startPointA++;
            startPointB++;
            startPointC++;
            counter = counter + 6;
        }
        //Calc the wraparound
        tris[counter] = startPointA;
        tris[counter + 1] = startPointB;
        tris[counter + 2] = startPointC - numOfFacesToCalc;

        tris[counter + 3] = startPointA;
        tris[counter + 4] = startPointB - (numOfFacesToCalc - 1);
        tris[counter + 5] = startPointA - (numOfFacesToCalc - 1);
        counter = counter + 6;
    }

    void calcEndCaps()
    {
        frontCap();
        backCap();
    }
    void frontCap()
    {
        int counterA = 1;
        for (int i = 0; i < nodesPerSegment - 2; i++)
        {

            tris[counter] = 0;
            tris[counter + 1] = counterA;
            tris[counter + 2] = counterA + 1;
            counterA = counterA + 1;
            counter = counter + 3;
        }
    }

    void backCap()
    {
        int mainPoint = Inputpoints.Count - 1;
        int counterA = Inputpoints.Count - 2;
        for (int i = 0; i < nodesPerSegment - 2; i++)
        {

            tris[counter] = mainPoint;
            tris[counter + 1] = counterA;
            tris[counter + 2] = counterA - 1;
            counterA = counterA - 1;
            counter = counter + 3;
        }
    }


}

