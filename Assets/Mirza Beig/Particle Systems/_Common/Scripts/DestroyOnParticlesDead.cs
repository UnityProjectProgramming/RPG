
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

        public class DestroyOnParticlesDead : ParticleSystems
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

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

                // ...

                onParticleSystemsDeadEvent += onParticleSystemsDead;
            }

            // ...

            void onParticleSystemsDead()
            {
                Destroy(gameObject);
            }

            // ...

            protected override void Update()
            {
                base.Update();
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
