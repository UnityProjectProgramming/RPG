using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTrigger: MonoBehaviour
{
    public string nextLevel;
    
    public GameObject Text;
    

    void Start()
    {
        Text.SetActive(false);
    }
    void OnTriggerStay(Collider otherObjects)
    {
        if (otherObjects.gameObject.tag == "Player")
        {
            Text.SetActive(true);
            if (Input.GetButtonDown("Press"))
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
    void OnTriggerExit(Collider otherObjects)
    {
        if (otherObjects.gameObject.tag == "Player")
        {
            Text.SetActive(false);
        }
    }
}
