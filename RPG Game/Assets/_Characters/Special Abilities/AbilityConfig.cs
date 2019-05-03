using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityEngine.UI;

namespace RPG.Characters
{

    public abstract class AbilityConfig : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] float cooldown = 3.0f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AnimationClip abilityAnimation = null;
        [SerializeField] AudioClip[] audioClips = null;


        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);
        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public AbilityBehaviour GetAbilityBehaviour()
        {
            return behaviour;
        }
        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }
        public float GetEnergyCost()
        {
            return energyCost;
        }
        public float GetCoolDown()
        {
            return cooldown;
        }
        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }
        public AnimationClip GetAbilityAnimation()
        {
            return abilityAnimation;
        }
        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
