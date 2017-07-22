
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Scripting
    {

        namespace Effects
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            public class VortexParticleAffectorGPU : ParticleAffectorGPU
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                Vector3 axisOfRotation;

                [Header("Affector Controls")]

                // Useful if particle affector and particle system
                // are on the same game object, and you need a seperate 
                // rotation for the system, and the affector, but don't 
                // want to make the two different game objects.

                public Vector3 axisOfRotationOffset = Vector3.zero;

                // =================================	
                // Functions.
                // =================================

                // ...

                protected override void Awake()
                {
                    base.Awake();
                }

                // ...

                protected override void Start()
                {
                    base.Start();
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }
                // ...

                protected override void perParticleSystemSetup()
                {
                    Vector3 transformUpWithOffset =
                        Quaternion.Euler(axisOfRotationOffset) * transform.up;

                    if (currentParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World)
                    {
                        axisOfRotation = transformUpWithOffset;
                    }
                    else
                    {
                        // If local space, un-do rotation of particle system so that even if they are local,
                        // the system will rotate around the affector axis. Without this, the axis of rotation
                        // would end up being a mix of the rotations of the affector, and the particle system...

                        // First, get the full rotation by adding the system and the effector rotations.

                        Quaternion transformRotation = transform.rotation;

                        Quaternion transformRotationWithOffset = Quaternion.Euler(axisOfRotationOffset) * transformRotation;
                        Quaternion totalRotation = transformRotation * currentParticleSystem.transform.rotation;

                        // Get the difference (q1 - q2) to that rotation for the relative rotation required to compensate
                        // for the rotational difference between the system and the affector. 

                        Quaternion relative = Quaternion.Inverse(totalRotation) * transformRotationWithOffset;
                        axisOfRotation = (relative * Quaternion.Euler(axisOfRotationOffset)) * transformUpWithOffset;
                    }
                }

                // ...

                protected override void setShaderData()
                {
                    base.setShaderData();

                    // ...

                    shader.SetVector("axisOfRotation", axisOfRotation);
                }

                // ...

                protected override void LateUpdate()
                {
                    base.LateUpdate();
                }

                // ...

                protected override void OnDrawGizmosSelected()
                {
                    base.OnDrawGizmosSelected();

                    // ...

                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position + offset, (transform.position + offset) + (transform.up * scaledRadius));
                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================	
// --END-- //
// =================================
