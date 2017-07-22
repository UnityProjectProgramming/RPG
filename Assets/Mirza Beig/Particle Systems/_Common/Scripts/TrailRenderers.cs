
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

        //[RequireComponent(typeof(TrailRenderer))]

        public class TrailRenderers : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            [HideInInspector]
            public TrailRenderer[] trailRenderers;

            // =================================	
            // Functions.
            // =================================

            // ...

            protected virtual void Awake()
            {

            }

            // ...

            protected virtual void Start()
            {
                trailRenderers = GetComponentsInChildren<TrailRenderer>();
            }

            // ...

            protected virtual void Update()
            {

            }

            // ...

            public void setAutoDestruct(bool value)
            {
                for (int i = 0; i < trailRenderers.Length; i++)
                {
                    trailRenderers[i].autodestruct = value;
                }
            }

            // ...

            //void Update()
            //{

            //}

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
