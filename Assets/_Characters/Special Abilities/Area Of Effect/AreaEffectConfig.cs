using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Of Effects"))]
    public class AreaEffectConfig : SpecialAbility
    {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5;
        [SerializeField] float damageToEachTarget = 15;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<AreaEffectBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;

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
