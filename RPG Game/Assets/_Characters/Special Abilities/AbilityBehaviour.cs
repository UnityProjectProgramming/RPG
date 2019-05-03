using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;

        const float PARTICLE_CLEAN_UP_DELAY = 20.0f;
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        public bool canCastAbility = true;
        protected float timeUsed;

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }
        public abstract void Use(GameObject target = null);

        protected void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected IEnumerator StartCooldown(AbilityBehaviour behaviour)
        {
            float coolDown = config.GetCoolDown();
            canCastAbility = false;
            // Start ui cooldown.
            yield return new WaitForSeconds(coolDown);
            canCastAbility = true;
            Debug.Log("Finished Cooldown");
        }

        protected IEnumerator StartUICooldown(int abilityIndex, float time)
        {
            Image[] cooldownImages = GetComponent<SpecialAbilities>().GetCooldownImageAbilities();
            cooldownImages[abilityIndex].fillAmount = 1;
            float fillAmount = 0;
            while(cooldownImages[abilityIndex].fillAmount >= 0.01)
            {
                float currentTime = Time.time;
                fillAmount = (config.GetCoolDown() - (currentTime - time)) / config.GetCoolDown();
                Debug.Log("Fill Amount = " + fillAmount);
                cooldownImages[abilityIndex].fillAmount = fillAmount;
                yield return new WaitForEndOfFrame();
            }
        }

        protected void PlayAbilityAnimation()
        {
            var animatorOverrideController = GetComponent<Character>().GetAnimatorOverride();
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = config.GetAbilityAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound); 
        }
    }

}
