using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Gameplay
{
    public class LootChest : MonoBehaviour, ISaveable
    {
        // TODO: Play Open Animation
        // TODO: Play Sound
        // TODO: Play Particle Effect
        // TODO: Implement ISaveable


        [Header("Rewards")]

        [SerializeField] GameObject rewardPrefab;
        [SerializeField] [Range(0, 1000)] int experiencePointAmount;
        [SerializeField] [Range(0, 1000)] int goldAmount;

        [Space]

        [Header("General")]   
        [SerializeField] AudioClip audioClip;
        [SerializeField] ParticleSystem particleSystem;
        
        const string OPEN_CHEST_TRIGGER = "OpenLootChest";
        private bool isOpened = false;

        public IEnumerator OpenLootChest()
        {
            if(!isOpened)
            {
                isOpened = true;
                Animator anim = GetComponent<Animator>();
                anim.SetTrigger("OpenLootChest");
                GetComponent<AudioSource>().PlayOneShot(audioClip);
                particleSystem.Play();
                yield return anim.GetCurrentAnimatorStateInfo(0).length;
                GiveReward();
            }
        }

        private void GiveReward()
        {

            Debug.Log("Giving Player Rewards");
        }

        public object CaptureState()
        {
            return isOpened;
        }

        public void RestoreState(object state)
        {
            isOpened = (bool)state;
            if(isOpened)
            {
                GetComponent<Animator>().SetTrigger("OpenLootChest");
            }
        }
    }
}

