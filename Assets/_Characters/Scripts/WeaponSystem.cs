using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField] WeaponConfig currentWeaponConfig;

        [Header("Damage Setup")]
        [SerializeField] float baseDamage = 10f;
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

        [Header("Particle Setup")]
        [SerializeField] ParticleSystem criticalHitParticle;


        GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;
        float lastHitTime = 0f;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            //TODO check continously if we should still be attacking 
            bool targetIsDead;
            bool targetIsOutOfRange;
            if( target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                // test if target is dead
                targetIsDead = target.GetComponent<HealthSystem>().healthAsPercentage <= Mathf.Epsilon;
                // test if target is out of range
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }
            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = (characterHealth <= Mathf.Epsilon);

            if(characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.grip.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.grip.localRotation;
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            print("Attacking  " + targetToAttack);
            //TODO use a repeat attack  co-routine.
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly()
        {
            //determine if alive (Attacker or defender)
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            //while still alive
            while (attackerStillAlive && targetStillAlive)
            {
                //know how often to attack
                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = weaponHitPeriod + character.GetAnimatorSpeedMultiplier();
                //if time to hit again
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                
                if(isTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce()
        {
            //look at the other person;
            transform.LookAt(target.transform);
            //trigger attack animation
            animator.SetTrigger(ATTACK_TRIGGER);
            //deal damage
            float damageDelay = 1.0f; //TODO get from weapon
            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        private void SetAttackAnimation()
        {
            //protect against animator override controller
            if(!character.GetAnimatorOverride())
            {
                Debug.Break();
                Debug.LogAssertion("Please Provide " + gameObject.name + " with Animator Override Controller ");
            }
            else
            {
                var animatorOverrideController = character.GetAnimatorOverride();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAnimClip();
            }
        }


        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, ("No Domiannt hand found on the  ")+gameObject.name + ("  please add one ."));
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple Dominant hand scripts on the player , please remove one");
            return dominantHands[0].gameObject;
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator = GetComponent<Animator>();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            // allow critical hit 
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            if (isCriticalHit)
            {
                criticalHitParticle.Play();
                return damageBeforeCritical * criticalHitMultiplier;
            }
            else
            {
                return damageBeforeCritical;
            }
        }
    }
}
