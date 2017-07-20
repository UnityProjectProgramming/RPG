using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{


    public class SelfHealBehaviour : AbilityBehaviour
    {
        Player player;

        private AudioSource audioSource;

        void Start()
        {
            player = GameObject.FindObjectOfType<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            player.Heal((config as SelfHealConfig).GetHealAmount());
            audioSource.clip = config.GetAbilitySFX();
            audioSource.Play();
            PlayParticleEffect();
        }
    }
}
