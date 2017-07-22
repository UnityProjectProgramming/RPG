
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.UI;

//using System.Collections;

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

            public class DemoManager_XPTitles : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                LoopingParticleSystemsManager list;

                public Text particleCountText;
                public Text currentParticleSystemText;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {
                    (list = GetComponent<LoopingParticleSystemsManager>()).init();
                }

                // ...

                void Start()
                {
                    updateCurrentParticleSystemNameText();
                }

                // ...

                void Update()
                {
                    // Scroll through systems.

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        next();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        previous();
                    }
                }

                // ...

                void LateUpdate()
                {
                    // Update particle count display.

                    particleCountText.text = "PARTICLE COUNT: ";
                    particleCountText.text += list.getParticleCount().ToString();
                }

                // ...

                public void next()
                {
                    list.next();
                    updateCurrentParticleSystemNameText();
                }
                public void previous()
                {
                    list.previous();
                    updateCurrentParticleSystemNameText();
                }

                // ...

                void updateCurrentParticleSystemNameText()
                {
                    currentParticleSystemText.text = list.getCurrentPrefabName(true);
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
