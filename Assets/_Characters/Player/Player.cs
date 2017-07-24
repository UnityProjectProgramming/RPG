using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI; // TODO , Consider Re-Wiring.
using RPG.Core;
using RPG.Weapons;


namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoint = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Weapon weaponInUse;

        AudioSource audioSource;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle;

        //Temporary used serliazedField
        [SerializeField] AbilityConfig[] abilities;


        
        Enemy enemy = null;
        private float currentHealthPoint;
        private float lastHitTime = 0f;
        Animator animator;
        const String DEATH_TRIGGER = "Death";
        const String ATTACK_TRIGGER = "Attack";

        CameraRaycaster cameraRaycaster;


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AttachInitialAbilities();
        }

        private void AttachInitialAbilities()
        {
            for(int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }       
        }

        private void Update()
        {
            if (healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // AoE Special Ability
            {
                AttemptSpecialAbility(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) //Self Heal Special Ability
            {
                AttemptSpecialAbility(2);
            }
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoint = maxHealthPoint;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAnimClip();
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.grip.localPosition;
            weapon.transform.localRotation = weaponInUse.grip.localRotation;
            //TODO Move to Correct place and child to hand.
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No Domiannt hand found on the player , please add one .");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple Dominant hand scripts on the player , please remove one");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            //cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy; //Delegate
        }
        private void AttemptSpecialAbility(int abilityIndex)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyComponent.IsEnergyAvaliable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator = GetComponent<Animator>();
                enemy.TakeDamage(CalculateDamage());
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            // allow critical hit 
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + weaponInUse.GetAdditionalDamage();
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

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemyToSet.gameObject))
            {
                AttackTarget();
            }
            if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0);
            }
        }

        IEnumerator KillPlayer()
        {
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            SceneManager.LoadScene(0);
        }

        public float healthAsPercentage { get { return currentHealthPoint / maxHealthPoint; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoint = Mathf.Clamp(currentHealthPoint - damage, 0f, maxHealthPoint);
            audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.Play();
            if (currentHealthPoint <=0)
            {            
                StartCoroutine(KillPlayer());
            }
        }

        public void Heal(float healAmount)
        {
            currentHealthPoint = Mathf.Clamp(currentHealthPoint + healAmount, 0f, maxHealthPoint);
        }

        public void PutWeaponInHand(Weapon weaponConfig)
        {
            print("Putting In player hand : " + weaponConfig);
        }


    }
}
