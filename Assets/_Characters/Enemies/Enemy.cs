using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float attackRadious = 3f;
        [SerializeField] float chaseRadious = 6f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] GameObject projectileToUse = null;
        [SerializeField] GameObject projectileSocket = null;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        bool isAttacking = false;

        Player player = null;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            currentHealthPoint = maxHealthPoint;

        }

        private void Update()
        {
            if(player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this); //Destroy Enemy behaviour script (Enemy will still exist).
            }
            var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadious && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = UnityEngine.Random.Range(firingPeriodInSeconds - firingPeriodVariation,
                                                                 firingPeriodInSeconds + firingPeriodVariation);

                InvokeRepeating("FireProjectile", 0f, randomisedDelay);
            }

            if (distanceToPlayer > attackRadious)
            {
                isAttacking = false;
                CancelInvoke();
            }
            if (distanceToPlayer <= chaseRadious)
            {
               // aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
               // aiCharacterControl.SetTarget(transform);
            }
        }
        //TODO seperate out character firing Logic.
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponant = newProjectile.GetComponent<Projectile>();
            projectileComponant.damageCaused = damagePerShot;
            projectileComponant.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponant.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadious);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadious);
        }
    }
}
