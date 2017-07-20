using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{


    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {

        SelfHealConfig config;
        Player player;

        private AudioSource audioSource;
        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }

        void Start()
        {
            player = GameObject.FindObjectOfType<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            player.Heal(config.GetHealAmount());
            PlaySFX();
            PlayParticleEffect();
        }

        private void PlaySFX()
        {
            audioSource.clip = config.GetAbilitySFX();
            audioSource.Play();
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            //TODO , Decides if particle System attached to the Player
            prefab.transform.parent = transform;
            ParticleSystem myParticleSystem = GameObject.FindObjectOfType<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration + 5); // TODO remove Magic Number
        }
    }
}
