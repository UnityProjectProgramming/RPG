
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

            public class AttractionParticleAffector : ParticleAffector
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("Affector Controls")]

                public float arrivalRadius = 1.0f;
                public float arrivedRadius = 0.5f;

                float arrivalRadiusSqr;
                float arrivedRadiusSqr;

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

                protected override void LateUpdate()
                {
                    float uniformTransformScale = transform.lossyScale.x;

                    arrivalRadiusSqr = (arrivalRadius * arrivalRadius) * uniformTransformScale;
                    arrivedRadiusSqr = (arrivedRadius * arrivedRadius) * uniformTransformScale;

                    // ...

                    base.LateUpdate();
                }

                // ...

                protected override Vector3 getForce(GetForceParameters parameters)
                {
                    if (parameters.distanceToAffectorCenterSqr <= arrivedRadiusSqr)
                    {
                        return Vector3.zero;
                    }
                    else if (parameters.distanceToAffectorCenterSqr < arrivalRadiusSqr)
                    {
                        float inverseArrivalScaleNormalized = 1.0f - (parameters.distanceToAffectorCenterSqr / arrivalRadiusSqr);
                        return -parameters.scaledDirectionToAffectorCenter.normalized * inverseArrivalScaleNormalized;
                    }

                    return -parameters.scaledDirectionToAffectorCenter.normalized;
                }

                // ...

                protected override void OnDrawGizmosSelected()
                {
                    base.OnDrawGizmosSelected();

                    // ...

                    float uniformTransformScale = transform.lossyScale.x;

                    float arrivalRadius = this.arrivalRadius * uniformTransformScale;
                    float arrivedRadius = this.arrivedRadius * uniformTransformScale;

                    Vector3 center = transform.position + offset;

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(center, arrivalRadius);

                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(center, arrivedRadius);

                    //Gizmos.color = Color.white;
                    //Gizmos.DrawLine(currentParticleSystem.transform.position, center);
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
