using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour //no Idamageable because we are going fron interface to component
    {
        SpecialAbilities abilities;
        Character character;
        private bool isGamePaused;
        public LevelFlowManager levelFlowManager;
        WeaponSystem weaponSystem;

        private void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            isGamePaused = false;
            abilities = GetComponent<SpecialAbilities>();
            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy; //Delegate
            cameraRaycaster.onMouseOverpotentiallyWalkable += onMouseOverpotentiallyWalkable;
        }

        private void Update()
        {         
            ScanForAbilityKeyDown();         
            PauseGame();
        }

        void onMouseOverpotentiallyWalkable(Vector3 destination)
        {
            if(Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        private void ScanForAbilityKeyDown()
        {

            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject); 
            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            this.character.SetDestination(target.transform.position);
            while(!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack (EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        public void OnButtonEven()   //TODO , move to anotehr script
        {
            SceneManager.LoadScene(0);
        }

        private void PauseGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isGamePaused = !isGamePaused;
            }
            if (isGamePaused && Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                levelFlowManager.pauseGame.SetActive(true);

            }
            else if (!isGamePaused && Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                levelFlowManager.pauseGame.SetActive(false);
            }
        }
    }
}
