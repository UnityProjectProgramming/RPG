using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/WeaponConfig"))]
    public class WeaponConfig : ScriptableObject
    {

        public Transform grip;
        [SerializeField] GameObject weaponPrefab;
        //[SerializeField] GameObject projectileToUse;
        //[SerializeField] Transform projectileSocket;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = 0.5f;
        

        //public GameObject GetProjectileToUse()
        //{
        //    return projectileToUse;
        //}

        //public Transform GetProjectileSocket()
        //{
        //    return projectileSocket;
        //}

        public float GetMinTimeBetweenAnimationCycles()
        {
            return attackAnimation.length;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
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
            return attackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        // SO that assets pack can't cause bugs 
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}