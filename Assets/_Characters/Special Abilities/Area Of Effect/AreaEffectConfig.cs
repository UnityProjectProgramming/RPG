using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Of Effects"))]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5;
        [SerializeField] float damageToEachTarget = 15;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<AreaEffectBehaviour>();
        }

        public float GetRadius()
        {
            return radius;
        }
        public float GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }
    }

}
