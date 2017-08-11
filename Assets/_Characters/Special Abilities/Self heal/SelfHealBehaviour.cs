using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{


    public class SelfHealBehaviour : AbilityBehaviour
    {
        Player player;
        void Start()
        {
            player = GetComponent<Player>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            var playerHealth = GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetHealAmount());
            PlayAbilitySound();
            PlayParticleEffect();
        }
    }
}
