using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class LevelFlowManager : MonoBehaviour
    {

        public GameObject pauseGame;

        public void OnPlayPressed()
        {
            SceneManager.LoadScene(0);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void OnResumePressed()
        {
            pauseGame.SetActive(false);
            Time.timeScale = 1;
        }

    }
}
