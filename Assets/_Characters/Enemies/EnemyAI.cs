using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour//no Idamageable because we are going fron interface to component
    {

        [SerializeField] float chaseRadious = 6f;

        bool isAttacking = false; //TODO more rich state (idle, attacking, chating , patrol,...etc)
        float currentWeaponRange = 3f;

        PlayerMovement player = null;

        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        private void Update()
        {
            var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        //TODO We will simplify this Method in another way, we Can use this one if we didn't like the otherway.
        //void FireProjectile()
        //{
        //    GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        //    Projectile projectileComponant = newProjectile.GetComponent<Projectile>();
        //    projectileComponant.damageCaused = damagePerShot;
        //    projectileComponant.SetShooter(gameObject);
        //    Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        //    float projectileSpeed = projectileComponant.GetDefaultLaunchSpeed();
        //    newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        //}
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadious);
        }
    }
}
