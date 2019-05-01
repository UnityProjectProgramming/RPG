using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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


