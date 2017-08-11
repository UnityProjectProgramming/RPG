using UnityEngine; // TODO using System was removed.
using UnityEngine.AI;
using RPG.CameraUI; // TODO , Consider Re-Wiring.
using System;

namespace RPG.Characters
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
   
    public class Character : MonoBehaviour
    {
        //====Serliaized Section====

        [Header("Capsule Collider Settings")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0.0f, 0.8f, 0.0f);
        [SerializeField] float capsuleRadius = 0.2f;
        [SerializeField] float capsuleHeight = 1.6f;


        [Header("Setup Settings")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        
        [Header("Movement Properties")]
        [SerializeField] float moveSpeedMultiplier  = 0.5f;
        [SerializeField] float moveThreshold        = 1.0f;
        [SerializeField] float movingTurnSpeed      = 360;
        [SerializeField] float stationaryTurnSpeed  = 180;
        [SerializeField] float stoppingDistance     = 1.0f;
        [SerializeField] float runCycleLegOffset    = 0.2f;
        [SerializeField] float animSpeedMultiplier  = 1.5f;


        //====Private Section====
        float turnAmount;
        float forwardAmount;

        GameObject walkTarget;
        Vector3 clickPoint;
        NavMeshAgent agent;
        Rigidbody myRigidBody;
        Animator animator;


        private void Awake()
        {
            AddRequierdComponents();
        }

        private void AddRequierdComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = capsuleRadius;
            capsuleCollider.height = capsuleHeight;
            capsuleCollider.center = colliderCenter;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = true;
        }

        void Start()
        {
            // Ray-Casting and Applying Delegates
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverpotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;        

            myRigidBody = GetComponent<Rigidbody>();
            myRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;

            walkTarget = new GameObject("walkTarget");
        }

        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
               Move(agent.desiredVelocity);
            }
            else
            {
               Move(Vector3.zero);
            }
        }

        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        public void Kill()
        {
            // To allow death signaling.
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
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animSpeedMultiplier;
        }

        private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))// left mouse Or Right click
            {
                agent.SetDestination(enemy.transform.position);
            } 
        }

        //Call-Back (google the function name if you need help)
        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
                velocity.y = myRigidBody.velocity.y;
                myRigidBody.velocity = velocity;
            }
        }

    }
}



