
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;


namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        
        [SerializeField] Image energyOrb = null;
    
        [SerializeField] float maxEnergyPoint = 100;
        [SerializeField] float regenPointsPerSeconds = 2;
        float currentEnegryPoint;



        // Use this for initialization
        void Start()
        {
            currentEnegryPoint = maxEnergyPoint;
        }
        void Update()
        {
            RegenHealthBar();
        }

        private void RegenHealthBar()
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

        public bool IsEnergyAvaliable(float amount)
        {
            return amount <= currentEnegryPoint;
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoint = currentEnegryPoint - amount;
            currentEnegryPoint = Mathf.Clamp(newEnergyPoint, 0, maxEnergyPoint);
            UpdateEnergyBar();
        }
        
       void UpdateEnergyBar()
        {
            energyOrb.fillAmount = EnergyAsPercent();
        }

        float EnergyAsPercent()
        {
            return currentEnegryPoint / maxEnergyPoint;
        }
    }
}
