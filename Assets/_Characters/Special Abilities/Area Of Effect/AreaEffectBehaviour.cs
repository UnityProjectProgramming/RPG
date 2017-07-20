using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : AbilityBehaviour {
        
        AreaEffectConfig config;
        AudioSource audioSource;

        public void SetConfig (AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }
        // Use this for initialization
        void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlaySFX();
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            //static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                    transform.position,
                    config.GetRadius(),
                    Vector3.up,
                    config.GetRadius()
                );
            foreach (RaycastHit hit in hits)
            {
                var damagable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damagable != null && !hitPlayer)
                {
                    float damageToTake = useParams.baseDamage + config.GetDamageToEachTarget();
                    damagable.TakeDamage(damageToTake);
                }
            }
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
            ParticleSystem myParticleSystem = GameObject.FindObjectOfType<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration + 5); //TODO remove Magic Number
        }
    }
}
