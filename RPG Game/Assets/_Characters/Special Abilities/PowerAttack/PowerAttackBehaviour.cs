using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target)
        {
            DealDamage(target);
            PlayAbilitySound();
            PlayParticleEffect();
        }

        private void DealDamage(GameObject target)
        {
            float damageToTake = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToTake);
        }
    }
}
