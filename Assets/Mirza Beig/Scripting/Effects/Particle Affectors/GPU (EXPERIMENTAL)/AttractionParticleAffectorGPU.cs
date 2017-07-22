
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

            public class AttractionParticleAffectorGPU : ParticleAffectorGPU
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

                protected override void setShaderData()
                {
                    base.setShaderData();

                    // ...

                    shader.SetFloat("arrivalRadiusSqr", arrivalRadiusSqr);
                    shader.SetFloat("arrivedRadiusSqr", arrivedRadiusSqr);
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

                protected override void OnDrawGizmosSelected()
                {
                    base.OnDrawGizmosSelected();

                    // ...

                    float uniformTransformScale = transform.lossyScale.x;

                    float arrivalRadius = this.arrivalRadius * uniformTransformScale;
                    float arrivedRadius = this.arrivedRadius * uniformTransformScale;

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position + offset, arrivalRadius);

                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position + offset, arrivedRadius);
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
