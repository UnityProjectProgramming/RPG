using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {

        public Transform grip;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip weaponAnimation;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float additionalDamage = 10f;

        public float GetMinTimeBetweenHits()
        {
            //TODO consider whether when we take animation time in account.
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange ()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }
        public AnimationClip GetAnimClip()
        {
            RemoveAnimationEvents();
            return weaponAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        // SO that assets pack can't cause bugs 
        private void RemoveAnimationEvents()
        {
            weaponAnimation.events = new AnimationEvent[0];
        }
    }
}