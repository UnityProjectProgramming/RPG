
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections.Generic;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Scripting
    {

        namespace Effects
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            [RequireComponent(typeof(ParticleSystem))]

            public class ParticleLights : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...                

                ParticleSystem ps;
                List<Light> lights = new List<Light>();

                //public LightType type = LightType.Point;

                public float scale = 2.0f;

                [Range(0.0f, 8.0f)]
                public float intensity = 8.0f;

                public Color colour = Color.white;

                [Range(0.0f, 1.0f)]
                public float colourFromParticle = 1.0f;

                public LightShadows shadows = LightShadows.None;

                GameObject template;

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
                    ps = GetComponent<ParticleSystem>();

                    template = new GameObject();
                    template.transform.SetParent(transform);

                    template.name = "Template";
                }

                // ...

                void Update()
                {

                }

                // ...

                void LateUpdate()
                {
                    ParticleSystem.Particle[] particles =
                        new ParticleSystem.Particle[ps.particleCount];

                    int numOfParticles = ps.GetParticles(particles);

                    if (lights.Count != numOfParticles)
                    {
                        for (int i = 0; i < lights.Count; i++)
                        {
                            Destroy(lights[i].gameObject);
                        }

                        lights.Clear();

                        for (int i = 0; i < numOfParticles; i++)
                        {
                            GameObject go = Instantiate(template, transform) as GameObject;
                            go.name = "- " + (i + 1).ToString();

                            lights.Add(go.AddComponent<Light>());
                        }
                    }

                    ParticleSystem.MainModule m = ps.main;
                    bool worldSpace = m.simulationSpace == ParticleSystemSimulationSpace.World;

                    for (int i = 0; i < numOfParticles; i++)
                    {
                        ParticleSystem.Particle p = particles[i];

                        Light light = lights[i];

                        //light.type = type;

                        //if (type == LightType.Spot)
                        //{
                        //    light.transform.rotation = Quaternion.Euler(p.rotation3D);
                        //}

                        light.range = p.GetCurrentSize(ps) * scale;
                        light.color = Color.Lerp(colour, p.GetCurrentColor(ps), colourFromParticle);

                        light.intensity = intensity;

                        light.shadows = shadows;

                        light.transform.position = worldSpace ? p.position : ps.transform.TransformPoint(p.position);
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
