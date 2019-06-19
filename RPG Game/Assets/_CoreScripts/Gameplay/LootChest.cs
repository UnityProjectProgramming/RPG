using RPG.Characters;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Gameplay
{
    public enum RewardType { None, Health, PowerBoost, Weapon}
    public class LootChest : MonoBehaviour, ISaveable
    {
        [Header("Rewards")]

        [SerializeField] GameObject rewardPrefab;
        [SerializeField] [Range(0, 1000)] int experiencePointAmount;
        [SerializeField] [Range(0, 1000)] int goldAmount;
        [SerializeField] RewardType rewardType = RewardType.None;
        [SerializeField] ParticleSystem boostDamageAura;
        [SerializeField] ParticleSystem boostDamageSwordParticles;
        [SerializeField] ParticleSystem healEffect;


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
            GameObject player = GameObject.FindWithTag("Player");
            // Give player experience and gold
            switch (rewardType)
            {
                case RewardType.Health:
                    // Give player health
                    player.GetComponent<HealthSystem>().Heal(80.0f);
                    healEffect.Play();
                    break;
                case RewardType.PowerBoost:
                    // give player more attack damage for a period amount of time
                    StartCoroutine(player.GetComponent<WeaponSystem>().BaseDamageBoostWithTime(60.0f, 15, boostDamageAura, boostDamageSwordParticles));                       
                    break;
                case RewardType.Weapon:
                    // spawn new weapon for the player
                    break;
                default:
                    break;
            }
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

