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
        [SerializeField] bool isProjectileWeapon = false;

        [Header("Damage Setup")]
        [SerializeField] float baseDamage = 10f;
        [Range(0.0f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
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

        public void StopAttacking()
        {
            animator.StopPlayback();
            StopAllCoroutines();
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        public IEnumerator BaseDamageBoostWithTime(float effectEndTime, float boostAmount ,ParticleSystem particleSystemAura, ParticleSystem particleSystemSword)
        {
            float originalBaseDamage = baseDamage;
            float newBaseDamage = baseDamage + boostAmount;
            baseDamage = newBaseDamage;

            particleSystemAura.Play();
            particleSystemSword.Play();

            yield return new WaitForSeconds(effectEndTime);

            particleSystemAura.Stop();
            particleSystemSword.Stop();

            baseDamage = originalBaseDamage;
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
                var animationClip = currentWeaponConfig.GetAnimClip();
                float animationClipTime = animationClip.length  / character.GetAnimatorSpeedMultiplier();
                //float timeToWait = animationClipTime + currentWeaponConfig.GetMinTimeBetweenAnimationCycles();
                float timeToWait = currentWeaponConfig.GetTimeToWaitBetweenHits();
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
            float damageDelay = currentWeaponConfig.GetDamageDelay();
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

        public bool GetIsProjectileWeapon()
        {
            return isProjectileWeapon;
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
            //Assert.IsFalse(numberOfDominantHands <= 0, ("No Domiannt hand found on the  ")+gameObject.name + ("  please add one ."));
            //Assert.IsFalse(numberOfDominantHands > 1, "Multiple Dominant hand scripts on the player , please remove one");
            if(dominantHands[0].gameObject)
            {
                return dominantHands[0].gameObject;
            }
            return gameObject;
        }


        private float CalculateDamage()
        {
            // allow critical hit 
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            if (isCriticalHit)
            {
                if(criticalHitParticle)
                {
                    criticalHitParticle.Play();
                    Debug.LogWarning("Critical Hit Prefab is not Assigned For : " + gameObject.name + "..  Please Assign 1.");
                }
                return damageBeforeCritical * criticalHitMultiplier;
            }
            else
            {
                return damageBeforeCritical;
            }
        }

        //TODO , Consider re-working this Method to make it fire an arrow that if it enterd the Player it will dealy damage
        // otherwise , the damage wont be dealt , also considering making damage on AnimationEvents.
        //void FireProjectile()
        //{
        //    //if (GetIsProjectileWeapon() != true) { return; }
        //    var projectileToUse = currentWeaponConfig.GetProjectileToUse();
        //    var projectileSocket = currentWeaponConfig.GetProjectileSocket();
        //    GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.position, Quaternion.identity);
        //    Vector3 aimOffset = new Vector3(1, 1, 1);
        //    Vector3 unitVectorToPlayer = (transform.position + aimOffset - projectileSocket.transform.position).normalized;
        //    float projectileSpeed = 20;
        //    newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        //}
    }
}
