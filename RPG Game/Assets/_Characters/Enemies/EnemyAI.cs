using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour//no Idamageable because we are going from interface to component
    {
        [Header("Waypoint System")]
        [SerializeField] float chaseRadious = 6f;
        [SerializeField] float waypointTolerance = 1.5f;

        [Header("Patrol Path")]
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointDwellTimeMin = 2.0f;
        [SerializeField] float waypointDwellTimeMax = 5.0f;

        [SerializeField] enum State { idle, attacking, patrolling, chasing };
        [SerializeField] State state = State.idle;


        float distanceToPlayer;
        float currentWeaponRange = 3f;
        int nextWaypointIndex;
        PlayerControl player = null;
        Character character;
        WeaponSystem weaponSystem;


        private void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
            weaponSystem = GetComponent<WeaponSystem>();

        }

        private void Update()
        {
            if(!player)
            {
                return;
            }

            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if (distanceToPlayer > chaseRadious && state != State.patrolling)
            {
                //stop what we'are doing
                StopAllCoroutines();
                //Stop weaponSystem Coroutine.
                // weaponSystem.StopAllCoroutines();
                //start patrolling
                StartCoroutine(Patrol());
            }
            if (distanceToPlayer <=  chaseRadious && state != State.chasing)
            {
                //stop what we'are doing
                StopAllCoroutines();
                //weaponSystem.StopAllCoroutines();
                //start chasing
                StartCoroutine(ChasePlayer());
            }
            if(distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                //stop what we're doing
                StopAllCoroutines();
                //start attacking
                StartCoroutine(AttackPlayer());
            }
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;

            while(patrolPath != null)
            {
                //work out where to go next
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                //tell character to go there
                character.SetDestination(nextWaypointPos);
                //cycle waypoints
                CycleWaypointWhenClose(nextWaypointPos);
                //wait at a waypoint
                float randWaypointDwellTime = UnityEngine.Random.Range(waypointDwellTimeMin, waypointDwellTimeMax);
                yield return new WaitForSeconds(randWaypointDwellTime); 
            }       
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator AttackPlayer()
        {
            state = State.attacking;
            while(distanceToPlayer <= currentWeaponRange)
            {
                weaponSystem.AttackTarget(player.gameObject);
                yield return new WaitForEndOfFrame();
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if(Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }


        public void SetPatrolPath(WaypointContainer patrolToSet)
        {
            patrolPath = patrolToSet;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadious);
        }


    }
}
