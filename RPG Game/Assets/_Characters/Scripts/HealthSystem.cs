using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {
        // Serlized
        [SerializeField] float maxHealthPoint        = 100f;
        [SerializeField] float deathVanishSeconds    = 2.0f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        //Private
        const string DEATH_TRIGGER = "Death";
        private float currentHealthPoint ;
        Animator animator;
        AudioSource audioSource;
        Character characterMovement;
        public float healthAsPercentage { get { return currentHealthPoint / maxHealthPoint; } }



        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();
            currentHealthPoint = maxHealthPoint;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        void UpdateHealthBar()
        {
            if(healthBar)   // Enemies May not have health bars to update.
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoint - damage <= 0);
            currentHealthPoint = Mathf.Clamp(currentHealthPoint - damage, 0f, maxHealthPoint);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);
            if (characterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }

        public void Heal(float healAmount)
        {
            currentHealthPoint = Mathf.Clamp(currentHealthPoint + healAmount, 0f, maxHealthPoint);
        }

        IEnumerator KillCharacter()
        {
            characterMovement.Kill();
            characterMovement.GetCapsuleCollider().enabled = false; // Disabling the collider so when the enemy dies the player can path throgh thier dead bodys and not to have a glitch.
            animator.SetTrigger(DEATH_TRIGGER);
            var playerComponent = GetComponent<PlayerControl>();
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            characterMovement.GetNavMeshAgent().speed = 0;
            yield return new WaitForSecondsRealtime(audioSource.clip.length);
            if (playerComponent && playerComponent.isActiveAndEnabled) // relying on Lazy Evaluation (Google if you need help)
            {
                SceneManager.LoadScene(0);
            }
            else // Assume is enemy for now, reconsider other NPCs Later 
            {
                characterMovement.GetNavMeshAgent().speed = 0;
                DestroyObject(gameObject, deathVanishSeconds + audioSource.clip.length); // TODO use obj.Destory instead.
            }
        }
    }
}