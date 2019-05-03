using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{


    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerControl player;
        void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        public override void Use(GameObject target)
        {
            if(canCastAbility)
            {
                var playerHealth = GetComponent<HealthSystem>();
                playerHealth.Heal((config as SelfHealConfig).GetHealAmount());
                PlayAbilitySound();
                PlayParticleEffect();
                PlayAbilityAnimation();
                float timeSpellCasted = Time.time;
                StartCoroutine(StartCooldown(this));
                StartCoroutine(StartUICooldown(2, timeSpellCasted));

            }
        }
    }
}
