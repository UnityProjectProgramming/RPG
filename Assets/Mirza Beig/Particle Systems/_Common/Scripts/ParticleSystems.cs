
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

        public class ParticleSystems : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================



            // =================================	
            // Variables.
            // =================================

            public ParticleSystem[] particleSystems { get; set; }

            // Event delegates.

            public delegate void onParticleSystemsDeadEventHandler();
            public event onParticleSystemsDeadEventHandler onParticleSystemsDeadEvent;

            // =================================	
            // Functions.
            // =================================

            // ...

            protected virtual void Awake()
            {
                particleSystems = GetComponentsInChildren<ParticleSystem>();
            }

            // ...

            protected virtual void Start()
            {

            }

            // ...

            protected virtual void Update()
            {

            }

            // ...

            protected virtual void LateUpdate()
            {
                if (onParticleSystemsDeadEvent != null)
                {
                    if (!isAlive())
                    {
                        onParticleSystemsDeadEvent();
                    }
                }
            }

            // ...

            public void reset()
            {
                //simulate(0.0f, true);

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].time = 0.0f;
                }
            }

            // ...

            public void play()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Play(false);
                }
            }

            // ...

            public void pause()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Pause(false);
                }
            }

            // ...

            public void stop()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Stop(false);
                }
            }

            // ...

            public void clear()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Clear(false);
                }
            }

            // ...

            public void setLoop(bool loop)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    ParticleSystem.MainModule m = particleSystems[i].main;
                    m.loop = loop;
                }
            }

            // ...

            public void setPlaybackSpeed(float speed)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    ParticleSystem.MainModule m = particleSystems[i].main;
                    m.simulationSpeed = speed;
                }
            }

            // ...

            public void simulate(float time, bool reset = false)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Simulate(time, false, reset);
                }
            }

            // ...

            public bool isAlive()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i])
                    {
                        if (particleSystems[i].IsAlive())
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // ...

            public bool isPlaying(bool checkAll = false)
            {
                if (particleSystems.Length == 0)
                {
                    return false;
                }
                else if (!checkAll)
                {
                    return particleSystems[0].isPlaying;
                }
                else
                {
                    for (int i = 0; i < 0; i++)
                    {
                        if (!particleSystems[i].isPlaying)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            // ...

            public int getParticleCount()
            {
                int pcount = 0;

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i])
                    {
                        pcount += particleSystems[i].particleCount;
                    }
                }

                return pcount;
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
