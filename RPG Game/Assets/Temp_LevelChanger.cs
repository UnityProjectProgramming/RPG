using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_LevelChanger : MonoBehaviour {


	void Update ()
    {
        // Teleport
		if(Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(5);
        }
        // Adventure
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(3);
        }
        // Village
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(4);
        }
    }
}
