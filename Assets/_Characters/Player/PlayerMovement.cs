using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.CameraUI;
// TODO , extract weaponSystem
namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour //no Idamageable because we are going fron interface to component
    {
        Enemy enemy = null;
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster;
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
            //cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
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

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemyToSet.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
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
