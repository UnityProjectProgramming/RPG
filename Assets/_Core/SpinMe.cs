using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpinMe : MonoBehaviour
    {

        [SerializeField]
        float xRotationsPerMinute = 1f;
        [SerializeField]
        float yRotationsPerMinute = 1f;
        [SerializeField]
        float zRotationsPerMinute = 1f;

        void Update()
        {
            float xDegreesPerFrame = (Time.deltaTime / 60) * xRotationsPerMinute * 360;
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = (Time.deltaTime / 60) * yRotationsPerMinute * 360;
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = (Time.deltaTime / 60) * zRotationsPerMinute * 360;

            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}