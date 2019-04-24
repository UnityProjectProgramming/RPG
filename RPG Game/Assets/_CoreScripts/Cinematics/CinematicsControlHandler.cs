using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
            print("DisableControl");
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            print("EnableControl");
        }
    }

}
