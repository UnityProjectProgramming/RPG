using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour
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
    }
}


