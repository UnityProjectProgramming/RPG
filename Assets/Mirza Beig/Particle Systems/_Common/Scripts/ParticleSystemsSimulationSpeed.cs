
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

        public class ParticleSystemsSimulationSpeed : ParticleSystems
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public float speed = 1.0f;

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

                // ...

                setPlaybackSpeed(speed);
            }

            // ...

            protected override void LateUpdate()
            {
                base.LateUpdate();
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
