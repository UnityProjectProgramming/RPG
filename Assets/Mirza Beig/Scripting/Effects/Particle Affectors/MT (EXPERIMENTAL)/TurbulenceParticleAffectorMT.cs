
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Threading;

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

            public class TurbulenceParticleAffectorMT : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                public float force = 1.0f;
                public float speed = 1.0f;

                new ParticleSystem particleSystem;

                //Vector3 particleSystemPosition;

                ParticleSystem.Particle[] particles;

                float randomX;
                float randomY;
                float randomZ;

                float offsetX;
                float offsetY;
                float offsetZ;

                float deltaTime;

                Thread t;
                readonly object locker = new object();

                //bool running;

                bool processing;
                bool isDoneAssigning;

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
                    particleSystem = GetComponent<ParticleSystem>();

                    randomX = Random.Range(-32.0f, 32.0f);
                    randomY = Random.Range(-32.0f, 32.0f);
                    randomZ = Random.Range(-32.0f, 32.0f);

                    //running = true;

                    t = new Thread(process);
                    t.Start();

                    isDoneAssigning = true;
                }

                // ...

                void LateUpdate()
                {
                    lock (locker)
                    {
                        if (!processing && isDoneAssigning)
                        {
                            // Init.

                            //print(1);

                            particles = new ParticleSystem.Particle[particleSystem.particleCount];

                            particleSystem.GetParticles(particles);

                            float time = Time.time;
                            deltaTime = Time.deltaTime;

                            offsetX = (time * speed) * randomX;
                            offsetY = (time * speed) * randomY;
                            offsetZ = (time * speed) * randomZ;

                            //particleSystemPosition = particleSystem.transform.position;

                            processing = true;
                            isDoneAssigning = false;

                            // Pause else particles may be lost or created between 
                            // time thread started, and time it finished.

                            // If not paused, then re-assigning particles from before
                            // may cause jitter as they are reset to the last known
                            // positions, while also replacing potentially new particles.

                            //particleSystem.Pause();
                        }
                    }

                    if (t.ThreadState == ThreadState.Stopped)
                    {
                        t = new Thread(process);
                        t.Start();
                    }

                    //process();

                    lock (locker)
                    {
                        if (!processing && !isDoneAssigning)
                        {
                            // Assign.

                            //print(3);
                            //print("-----");

                            particleSystem.SetParticles(particles, particles.Length);

                            isDoneAssigning = true;

                            //particleSystem.Play();
                        }
                    }
                }

                // ...

                void process()
                {
                    //while (running)
                    //{
                    lock (locker)
                    {
                        if (processing)
                        {
                            // Process.

                            //print(2);

                            for (int i = 0; i < particles.Length; i++)
                            {
                                ParticleSystem.Particle particle = particles[i];
                                //Vector3 particleWorldPosition = particleSystemPosition + particle.position;

                                Vector3 particlePosition = particle.position;

                                Vector3 force = new Vector3(

                                   Noise.perlin(offsetX + particlePosition.x, offsetX + particlePosition.y, offsetX + particlePosition.z),
                                   Noise.perlin(offsetY + particlePosition.x, offsetY + particlePosition.y, offsetY + particlePosition.z),
                                   Noise.perlin(offsetZ + particlePosition.x, offsetZ + particlePosition.y, offsetZ + particlePosition.z)

                                   ) * this.force;

                                force *= deltaTime;
                                particle.velocity += force;

                                particles[i] = particle;
                            }

                            processing = false;
                        }
                        //Thread.Sleep(10);
                    }

                    //Thread.Sleep(10);
                    //}
                }

                // ...

                void OnDisable()
                {
                    //running = false;
                }
                void OnApplicationQuit()
                {
                    //running = false;
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
