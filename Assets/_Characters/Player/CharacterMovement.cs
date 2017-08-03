using UnityEngine; // TODO using System was removed.
using UnityEngine.AI;
using RPG.CameraUI; // TODO , Consider Re-Wiring.

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]

    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1.0f;
        ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
        GameObject walkTarget;
        Vector3 clickPoint;
        NavMeshAgent agent;

        //private bool isInDirectMode = false;

        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;
            cameraRaycaster.onMouseOverpotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
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

    }
}


