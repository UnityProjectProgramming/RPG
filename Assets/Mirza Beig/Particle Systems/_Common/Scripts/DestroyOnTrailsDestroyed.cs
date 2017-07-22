
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

        public class DestroyOnTrailsDestroyed : TrailRenderers
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
            }

            // ...

            protected override void Update()
            {
                base.Update();

                // ...

                bool destroy = true;

                for (int i = 0; i < trailRenderers.Length; i++)
                {
                    if (trailRenderers[i] != null)
                    {
                        destroy = false; break;
                    }
                }

                if (destroy)
                {
                    Destroy(gameObject);
                }
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
