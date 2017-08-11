using UnityEngine; // TODO using System was removed.
using UnityEngine.AI;
using RPG.CameraUI; // TODO , Consider Re-Wiring.
using System;

namespace RPG.Characters
{
    [SelectionBase] //To selecet the root of the character component i.e the player
    public class Character : MonoBehaviour
    {
        //====Serliaized Section====

        [Header("Audio")]
        [Range(0.0f, 1.0f)] [SerializeField] float audoiSourceSpatialBlend = 0.5f;
  
        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0.0f, 0.8f, 0.0f);
        [SerializeField] float capsuleRadius = 0.2f;
        [SerializeField] float capsuleHeight = 1.6f;


        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        
        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier  = 0.5f;
        [SerializeField] float moveThreshold        = 1.0f;
        [SerializeField] float movingTurnSpeed      = 360;
        [SerializeField] float stationaryTurnSpeed  = 180;
        [SerializeField] float runCycleLegOffset    = 0.2f;
        [SerializeField] float animSpeedMultiplier  = 1.5f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float speed                = 1.0f;
        [SerializeField] float angularSpeed         = 120.0f;
        [SerializeField] float acceleration         = 8.0f;
        [SerializeField] float stoppingDistance     = 1.3f;
        [SerializeField] float obstcleAvoidanceRadius = 0.1f;

        //====Private Section====
        float turnAmount;
        float forwardAmount;

        GameObject walkTarget;
        Vector3 clickPoint;
        NavMeshAgent navMeshAgent;
        Rigidbody myRigidBody;
        Animator animator;


        private void Awake()
        {
            AddRequierdComponents();
        }

        private void AddRequierdComponents()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audoiSourceSpatialBlend;

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
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
            animator.applyRootMotion = true;
        }

        void Start()
        {
            // Ray-Casting and Applying Delegates
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverpotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;    
            
            walkTarget = new GameObject("walkTarget");
        }

        void Update()
        {
            if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
               Move(navMeshAgent.desiredVelocity);
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
                navMeshAgent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))// left mouse Or Right click
            {
                navMeshAgent.SetDestination(enemy.transform.position);
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



