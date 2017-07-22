
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        // =================================	
        // Classes.
        // =================================

        //[ExecuteInEditMode]
        [System.Serializable]

        public class TransformNoise : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public PerlinNoiseXYZ positionNoise;
            public PerlinNoiseXYZ rotationNoise;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Start()
            {
                positionNoise.init();
                rotationNoise.init();
            }

            // ...

            void Update()
            {
                transform.localPosition = positionNoise.xyz;
                transform.localEulerAngles = rotationNoise.xyz;
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

// =================================	
// --END-- //
// =================================
