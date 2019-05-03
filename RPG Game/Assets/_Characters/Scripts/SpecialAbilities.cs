using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
using RPG.Saving;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour, ISaveable
    {
        
        [SerializeField] Image energyBar;
        [SerializeField] float currentEnegryPoint = 100;
        [SerializeField] float regenPointsPerSeconds = 2;
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] AudioClip outOfEnergy;
        [Space]
        [Header("UI Cooldown")]
        [SerializeField] Image[] cooldownImageAbilities;
        [SerializeField] Text[] cooldownNumbers;

        float maxEnergyPoint = 100;
        AudioSource audioSource;


        float energyAsPercent { get { return currentEnegryPoint / maxEnergyPoint; } }
        
        // Use this for initialization
        void Start()
        {          
            audioSource = GetComponent<AudioSource>();
            AttachInitialAbilities();
            UpdateEnergyBar();
            DisableCooldownNumbers();
        }
        void Update()
        {
            RegenEnergyBar();
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public Image[] GetCooldownImageAbilities()
        {
            return cooldownImageAbilities;
        }

        public Text[] GetCooldownNumbers()
        {
            return cooldownNumbers;
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            //var energyComponent = GetComponent<SpecialAbilities>();
            bool canCastAbility = abilities[abilityIndex].GetAbilityBehaviour().canCastAbility;
            if (!canCastAbility) { return; }
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyCost <= currentEnegryPoint)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target);
            }
            else
            {
                if(!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(outOfEnergy);
                }
            }
        }

        private void DisableCooldownNumbers()
        {
            foreach (var cdNum in cooldownNumbers)
            {
                cdNum.gameObject.SetActive(false);
            }
        }

        private void RegenEnergyBar()
        {
            if (currentEnegryPoint != 100 && currentEnegryPoint < 100)
            {
                currentEnegryPoint += regenPointsPerSeconds * Time.deltaTime;
                UpdateEnergyBar();
            } else
            {
                return;
            }
        }
        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }
        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoint = currentEnegryPoint - amount;
            currentEnegryPoint = Mathf.Clamp(newEnergyPoint, 0, maxEnergyPoint);
            UpdateEnergyBar();
        }
        
       void UpdateEnergyBar()
        {
            if (!energyBar)
            {
                Debug.Log("No Energy bar assigned, please assign energy bar for " + gameObject.name);
                return;
            }
            energyBar.fillAmount = energyAsPercent;
        }

        public object CaptureState()
        {
            return currentEnegryPoint;
        }

        public void RestoreState(object state)
        {
            currentEnegryPoint = (float)state;
        }
    }
}
