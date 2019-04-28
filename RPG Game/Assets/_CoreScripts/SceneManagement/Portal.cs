using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationType
        {
            A, B, C, D, E, F
        }

        [Header("Scene Manager")]
        [SerializeField] int sceneToLoadIndex = -1;
        [SerializeField] Transform spawnTransform;
        [SerializeField] DestinationType destinationType;

        [Space]

        [Header("Fading")]
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(StartTransitioning());
            }
        }

        IEnumerator StartTransitioning()
        {
            if(sceneToLoadIndex < 0)
            {
                Debug.LogError("Scene To Load is not set correctly.");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();


            // Fade Out
            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerTransform(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            
            Destroy(gameObject);
        }

        void UpdatePlayerTransform(Portal otherPatrol)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPatrol.spawnTransform.position;
            player.transform.rotation = otherPatrol.spawnTransform.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        Portal GetOtherPortal()
        {
            
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destinationType != destinationType) continue;

                return portal;
            }

            return null;
        }

    }

}

