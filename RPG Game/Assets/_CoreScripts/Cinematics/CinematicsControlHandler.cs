using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Characters;
using RPG.CameraUI;

namespace RPG.Cinematics
{
    public class CinematicsControlHandler : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            // Using Action Event "Delegates" ... The observer pattern
            PlayableDirector playableDirector = GetComponent<PlayableDirector>();
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            GameObject Player = GameObject.FindWithTag("Player");
            Player.GetComponent<PlayerControl>().enabled = false;

            GameObject MainCamera = GameObject.FindWithTag("MainCamera");
            MainCamera.GetComponent<CameraRaycaster>().enabled = false;

            print("DisableControl");
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            GameObject Player = GameObject.FindWithTag("Player");
            Player.GetComponent<PlayerControl>().enabled = true;

            GameObject MainCamera = GameObject.FindWithTag("MainCamera");
            MainCamera.GetComponent<CameraRaycaster>().enabled = true;

            print("EnableControl");
        }
    }

}
