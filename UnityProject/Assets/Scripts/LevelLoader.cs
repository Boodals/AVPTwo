using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {


	if(Input.GetKeyDown(KeyCode.U))
        {
            print("Loading");
            SceneManager.LoadSceneAsync("SkyWorld", LoadSceneMode.Additive);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("UnLoading");
           
            SceneManager.UnloadScene("SkyWorld");
        }
    }
}
