using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandArea : MonoBehaviour {

    public int numberOFClutterItems;
    public int numberOfClumpItems;
    public List<GameObject> itemList;
    public List<GameObject> ClumpList;
    public List<GameObject> uprightClumpList;
    private List<GameObject> spawnedItems;

    // Use this for initialization
    void Start () {
        makeWorld();
	
	}

    void makeWorld()
    {
        //makeWorldClutter();
       // makeWorldClumps();

        SpawnWorld(itemList, numberOFClutterItems, 100,20,false);
        SpawnWorld(ClumpList, numberOfClumpItems, 100,20,true);

    }

    void makeWorldClutter()
    {
        for (int i = 0; i < numberOFClutterItems; i++)
        {
            GameObject Go = Instantiate(itemList[Random.Range(0, itemList.Count)]);
            Go.transform.position = Random.insideUnitSphere * 100;//new Vector3(Random.Range(0, areaX), Random.Range(0, areaY), Random.Range(0, areaZ));
            Go.transform.SetParent(transform);
            Go.transform.rotation = Random.rotation;
        }

        Collider[] closeColliders = Physics.OverlapSphere(transform.position, 10);
        foreach (Collider col in closeColliders)
        {
            col.gameObject.GetComponent<Renderer>().enabled = false;
        }

    }

    void makeWorldClumps()
    {
        for (int i = 0; i < numberOfClumpItems; i++)
        {
            GameObject Go = Instantiate(ClumpList[Random.Range(0, ClumpList.Count)]);
            Go.transform.position = Random.insideUnitSphere * 100;
            Go.transform.SetParent(transform);

            Go.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, Random.Range(0, 360), Random.Range(0, 360)), Vector3.right);
        }
        Collider[] closeColliders = Physics.OverlapSphere(transform.position, 20);
        foreach (Collider col in closeColliders)
        {
            col.gameObject.GetComponent<Renderer>().enabled = false;
        }


    }

    void SpawnWorld(List<GameObject> _inList,int _numberOfItems, int _sphereSize,int _sphereSizeInnter,bool _upright)
    {
        for (int i = 0; i < _numberOfItems; i++)
        {
            GameObject Go = Instantiate(_inList[Random.Range(0, _inList.Count)]);
            Go.transform.position = Random.insideUnitSphere * _sphereSize;
            Go.transform.SetParent(transform);

            if (!_upright)
            {
                Go.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, Random.Range(0, 360), Random.Range(0, 360)), Vector3.right);
            }
        }
        Collider[] closeColliders = Physics.OverlapSphere(transform.position, _sphereSizeInnter);
        foreach (Collider col in closeColliders)
        {
            col.gameObject.GetComponent<Renderer>().enabled = false;
        }


    }



}
