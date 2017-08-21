
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;


namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoint = 100;
        [SerializeField] float regenPointsPerSeconds = 2;
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] AudioClip outOfEnergy;
        
        float currentEnegryPoint;
        AudioSource audioSource;


        float energyAsPercent { get { return currentEnegryPoint / maxEnergyPoint; } }
        
        // Use this for initialization
        void Start()
        {          
            audioSource = GetComponent<AudioSource>();
            currentEnegryPoint = maxEnergyPoint;
            AttachInitialAbilities();
            UpdateEnergyBar();
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

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            //var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyCost <= currentEnegryPoint)
            {
                ConsumeEnergy(energyCost);
                print("Using Special Ability " + abilityIndex);  // TODO make work.
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
            energyBar.fillAmount = energyAsPercent;
        }

    }
}
