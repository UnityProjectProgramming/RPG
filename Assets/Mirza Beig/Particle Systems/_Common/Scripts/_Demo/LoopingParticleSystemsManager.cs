
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

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            public class LoopingParticleSystemsManager : ParticleManager
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

                    particlePrefabs[currentParticlePrefab].gameObject.SetActive(true);
                }

                // ...

                public override void next()
                {
                    particlePrefabs[currentParticlePrefab].gameObject.SetActive(false);

                    base.next();
                    particlePrefabs[currentParticlePrefab].gameObject.SetActive(true);
                }
                public override void previous()
                {
                    particlePrefabs[currentParticlePrefab].gameObject.SetActive(false);

                    base.previous();
                    particlePrefabs[currentParticlePrefab].gameObject.SetActive(true);
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                public override int getParticleCount()
                {
                    // Return particle count from active prefab.

                    return particlePrefabs[currentParticlePrefab].getParticleCount();
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

// =================================	
// --END-- //
// =================================
