using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour , ISpecialAbility
    {
        PowerAttackConfig config;
        AudioSource audioSource = null;

        public void SetConfig (PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            print("Power Attack Used by : " + gameObject.name);
            DealDamage(useParams);
            PlaySFX();
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToTake = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToTake);
        }

        private void PlaySFX()
        {
            audioSource.clip = config.GetAbilitySFX();
            audioSource.Play();
        }
        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            //TODO , Decides if particle System attached to the Player
            ParticleSystem myParticleSystem = GameObject.FindObjectOfType<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

    }
}
