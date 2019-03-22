using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {
    public GameObject LoadingScreen;
    public Slider slider;
    public Text progressText;
    private static int currentSceneIndex = 0;

	public void LoadLevel( int sceneIndex)
    {
        currentSceneIndex = sceneIndex;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public static int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";
            Debug.Log("Operations Prog: " + operation.progress);
            Debug.Log("Progress Text: " + slider.value);
            yield return null;
        }
    }
}
