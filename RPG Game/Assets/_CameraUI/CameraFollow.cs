using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {

        [SerializeField] float smoothSpeed;

        private GameObject player;

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void LateUpdate()
        {
            Vector3 desiredPos = player.transform.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
