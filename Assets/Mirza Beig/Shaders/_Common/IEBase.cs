
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Shaders
    {

        namespace ImageEffects
        {

            // =================================	
            // Classes.
            // =================================

            [ExecuteInEditMode]
            [System.Serializable]

            public class IEBase : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                protected Material material
                {
                    get
                    {
                        if (!_material)
                        {
                            _material = new Material(shader);
                            _material.hideFlags = HideFlags.HideAndDontSave;
                        }

                        return _material;
                    }

                }
                Material _material;

                protected Shader shader { get; set; }

                // ...

                new protected Camera camera
                {
                    get
                    {
                        if (!_camera)
                        {
                            _camera = GetComponent<Camera>();
                        }

                        return _camera;
                    }

                }
                Camera _camera;

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

                }

                // ...

                void OnRenderImage(RenderTexture source, RenderTexture destination)
                {

                }

                // ...

                protected void blit(RenderTexture source, RenderTexture destination)
                {
                    Graphics.Blit(source, destination, material);
                }

                // ...

                void OnDisable()
                {
                    if (_material)
                    {
                        DestroyImmediate(_material);
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
