using UnityEngine; // TODO using System was removed.
using UnityEngine.AI;
using RPG.CameraUI; // TODO , Consider Re-Wiring.

namespace RPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]

    public class PlayerMovement : MonoBehaviour
    {
        ThirdPersonCharacter m_Character = null;   // A reference to the ThirdPersonCharacter on the object
        AICharacterControl aiCharacterControl = null;
        CameraRaycaster cameraRaycaster = null;
        GameObject walkTarget = null;
        Vector3 clickPoint;

        //private bool isInDirectMode = false;

        void Start()
        {
            aiCharacterControl = GetComponent<AICharacterControl>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            m_Character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.onMouseOverpotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))// left mouse Or Right click
            {
                aiCharacterControl.SetTarget(enemy.transform);
            } 
        }

        //TODO make this get called again.
        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

            m_Character.Move(m_Move, false, false);
        }
    }
}


