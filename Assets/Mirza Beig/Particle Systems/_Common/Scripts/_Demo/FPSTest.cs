
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

            public class FPSTest : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                public int targetFPS1 = 60;
                public int targetFPS2 = 10;

                int previousVSyncCount;

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

                }

                // ...

                void Update()
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        Application.targetFrameRate = targetFPS2;

                        previousVSyncCount = QualitySettings.vSyncCount;
                        QualitySettings.vSyncCount = 0;
                    }
                    else if (Input.GetKeyUp(KeyCode.Space))
                    {
                        Application.targetFrameRate = targetFPS1;
                        QualitySettings.vSyncCount = previousVSyncCount;
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

}

// =================================	
// --END-- //
// =================================
