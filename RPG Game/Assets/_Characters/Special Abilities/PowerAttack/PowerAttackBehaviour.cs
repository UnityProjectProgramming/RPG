using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target)
        {
            if (canCastAbility)
            {
	            DealDamage(target);
	            transform.LookAt(target.transform);
	            PlayAbilitySound();
	            PlayParticleEffect();
	            PlayAbilityAnimation();
                float timeSpellCasted = Time.time;
                StartCoroutine(StartCooldown(this));
                StartCoroutine(StartUICooldown(0, timeSpellCasted));

            }

        }

        private void DealDamage(GameObject target)
        {
            float damageToTake = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToTake);
        }
    }
}
