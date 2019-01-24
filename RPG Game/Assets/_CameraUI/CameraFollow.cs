using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {
        private GameObject player;


        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(player.name);
        }

        private void Update()
        {
            transform.position = player.transform.position;
        }
    }
}
