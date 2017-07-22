using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : AbilityBehaviour {

        public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayAbilitySound();
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            //static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                    transform.position,
                    (config as AreaEffectConfig).GetRadius(),
                    Vector3.up,
                    (config as AreaEffectConfig).GetRadius()
                );
            foreach (RaycastHit hit in hits)
            {
                var damagable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damagable != null && !hitPlayer)
                {
                    float damageToTake = useParams.baseDamage + (config as AreaEffectConfig).GetDamageToEachTarget();
                    damagable.TakeDamage(damageToTake);
                }
            }
        }
    }
}
