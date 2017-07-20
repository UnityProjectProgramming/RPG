using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField]
        float projectileSpeed = 5;
        [SerializeField]
        GameObject shooter;
        [HideInInspector]
        public float damageCaused;
        //const float DESTROY_DELAY = 0.01f;



        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }
        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer)
            {
                DamageIfDamageable(collision);
            }
            else
            {
                return;
            }
        }

        private void DamageIfDamageable(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject);
        }
    }
}
