
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

        [RequireComponent(typeof(ParticleSystems))]
        public class IgnoreTimeScale : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            ParticleSystems particleSystems;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Awake()
            {

            }

            // ...

            void Start()
            {
                particleSystems = GetComponent<ParticleSystems>();
            }

            // ...

            void Update()
            {
                particleSystems.simulate(Time.unscaledDeltaTime);
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
