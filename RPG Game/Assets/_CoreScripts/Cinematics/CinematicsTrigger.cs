using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.SceneManagement;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour, ISaveable
    {
        // Private
        private bool hasTriggerd = false;

        private void OnTriggerEnter(Collider other)
        {
            if(!hasTriggerd && other.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                hasTriggerd = true;
                FindObjectOfType<SavingWrapper>().Save();
            }
        }

        public object CaptureState()
        {
            return hasTriggerd;
        }

        public void RestoreState(object state)
        {
            hasTriggerd = (bool)state;
        }

    }
}


