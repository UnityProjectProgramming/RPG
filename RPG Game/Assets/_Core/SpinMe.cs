using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SpinMe : MonoBehaviour
    {

        [SerializeField] float zRotationsPerMinute = 1f;

        void FixedUpdate()
        {
            float zDegreesPerFrame = (Time.deltaTime / 60) * zRotationsPerMinute * 360;
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}