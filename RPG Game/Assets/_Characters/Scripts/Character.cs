using UnityEngine; 
using UnityEngine.AI;
using System;
using RPG.Saving;

public enum EnemyType { None, HeavySoldire, Knight, Minion, Thug, Soldire, Dragon, NPC, Grunt, Warrior, Archer };

namespace RPG.Characters
{
    [SelectionBase] //To selecet the root of the character component i.e the player
    public class Character : MonoBehaviour, ISaveable
    {
        //====Serliaized Section====

        [Header("Audio")]
        [Range(0.0f, 1.0f)] [SerializeField] float audoiSourceSpatialBlend = 0.5f;

        [Space]

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        [SerializeField] [Range(0.1f, 1.0f)] float animForwardCap = 1.0f;

        [Space]

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0.0f, 0.8f, 0.0f);
        [SerializeField] float capsuleRadius = 0.2f;
        [SerializeField] float capsuleHeight = 1.6f;

        [Space]

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier  = 0.5f;
        [SerializeField] float moveThreshold        = 1.0f;
        [SerializeField] float movingTurnSpeed      = 360;
        [SerializeField] float stationaryTurnSpeed  = 180;
        [SerializeField] float runCycleLegOffset    = 0.2f;
        [SerializeField] float animSpeedMultiplier  = 1.5f;
        [SerializeField] bool rootMotion = false;

        [Space]

        [Header("Nav Mesh Agent")]
        [SerializeField] float speed                  = 1.0f;
        [SerializeField] float angularSpeed           = 120.0f;
        [SerializeField] float acceleration           = 8.0f;
        [SerializeField] float stoppingDistance       = 1.3f;
        [SerializeField] float obstcleAvoidanceRadius = 0.1f;

        [Header("Enemy Type")]
        [SerializeField] EnemyType enemyType = EnemyType.None;


        //====Private Section====
        float turnAmount;
        float forwardAmount;
        NavMeshAgent navMeshAgent;
        Rigidbody myRigidBody;
        Animator animator;
        bool isAlive = true;
        CapsuleCollider capsuleCollider;
        EnemyAI enemyAI;

        private void Awake()
        {
            AddRequierdComponents();
        }

        private void Start()
        {
            enemyAI = GetComponent<EnemyAI>();
        }

        public EnemyAI GetEnemyAI()
        {
            return enemyAI;
        }

        private void AddRequierdComponents()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audoiSourceSpatialBlend;

            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = capsuleRadius;
            capsuleCollider.height = capsuleHeight;
            capsuleCollider.center = colliderCenter;


            myRigidBody = gameObject.AddComponent<Rigidbody>();
            myRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
            navMeshAgent.angularSpeed = angularSpeed;
            navMeshAgent.acceleration = acceleration;
            navMeshAgent.stoppingDistance = stoppingDistance;
            navMeshAgent.radius = obstcleAvoidanceRadius;
            navMeshAgent.updateRotation = false;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updatePosition = true;


            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = rootMotion;
        }

        void Update()
        {
            if(!navMeshAgent)
            {
                Debug.Log("No Nav Mesh Agent Found");
                return;
            }
            if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
               Move(navMeshAgent.desiredVelocity);
            }
            else
            {
               Move(Vector3.zero);
            }
        }

        public void Kill()
        {
            CombatEvents.EnemyDied(this);
            isAlive = false;
        }
        
        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = worldPosition;
        }

        public AnimatorOverrideController GetAnimatorOverride()
        {
            return animatorOverrideController;
        }

        public float GetAnimatorSpeedMultiplier()
        {
            return animator.speed;
        }

        public CapsuleCollider GetCapsuleCollider()
        {
            return capsuleCollider;
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative.
            // turn amount and forward amount required to head in the desired direction.       
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMovement = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;
        }

        private void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount * animForwardCap, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animSpeedMultiplier;
        }

        //Call-Back (google the function name if you need help)
        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
                velocity.y = myRigidBody.velocity.y;
                myRigidBody.velocity = velocity;
                if(enemyType == EnemyType.Dragon)
                {
                    Debug.Log("Velocity : " + animator.deltaPosition);
                }
                
            }
        }

        public EnemyType GetEnemyType()
        {
            return enemyType;
        }

        public object CaptureState()
        {
            return new SerializableVector(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector vec = (SerializableVector)state;
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = vec.ToVector3();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}



