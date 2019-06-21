using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using UnityEngine.SceneManagement;

public class LoadCreditsScene : MonoBehaviour {

    // Use this for initialization
    [SerializeField] int sceneIndex;
    [SerializeField] GameObject gateGuard;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding");
        if (other.tag == "Player" && gateGuard.GetComponent<HealthSystem>().IsDead())
        {
            Debug.Log("StartLoading");
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {

        Fader fader = FindObjectOfType<Fader>();

        // Fade Out
        yield return fader.FadeOut(2);

        yield return SceneManager.LoadSceneAsync(sceneIndex);

        yield return new WaitForSeconds(1);
        yield return fader.FadeIn(2);


        Destroy(gameObject);

    }
}
