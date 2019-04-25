using UnityEngine;

namespace RPG.CameraUI
{
    public class Minimap : MonoBehaviour
    {
        private GameObject player;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void LateUpdate()
        {
            Vector3 newPos = player.transform.position;
            newPos.y = this.transform.position.y;
            transform.position = newPos;
        }

    }
}
