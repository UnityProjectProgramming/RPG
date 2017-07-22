
// =================================	
// Namespaces.
// =================================

using UnityEngine;

using System.Collections.Generic;
using System.Runtime.InteropServices;

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

            public abstract class ParticleAffectorGPU : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                [System.Serializable]
                public struct ParticleSystemData
                {
                    public Vector3 position;
                    public float externalForcesMultiplier;

                    Vector4 pad0;
                }

                // ...

                [System.Serializable]
                public struct ParticleData
                {
                    public Vector3 position;
                    public Vector3 velocity;

                    public int particleSystemIndex;

                    float pad0;
                };

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("Common Controls")]

                public float radius = Mathf.Infinity;
                public float force = 5.0f;

                public float scaledRadius
                {
                    get
                    {
                        return radius * transform.lossyScale.x;
                    }
                }

                protected float deltaTime;
                protected float radiusSqr;

                public Vector3 offset = Vector3.zero;

                public AnimationCurve scaleForceByDistance = new AnimationCurve(

                        new Keyframe(0.0f, 1.0f),
                        new Keyframe(1.0f, 1.0f)

                    );

                Texture2D scaleForceByDistanceTexture;

                // If (attached to a particle system): forces will be LOCAL.
                // Else if (attached to a particle system): forces will be SELECTIVE.
                // Else: forces will be GLOBAL.

                new ParticleSystem particleSystem;
                public ParticleSystem[] particleSystems = new ParticleSystem[0];

                // Current iteration of the particle systems when looping through all of them.
                // Useful to derived classes (like for the vortex particle affector).

                protected ParticleSystem currentParticleSystem;

                // Compute shader.

                [Header("GPU Data")]

                // ...

                [Range(0, 128)]
                public int LUTSteps = 8;

                // Only public for debugging and monitoring purposes.
                // These will be assigned directly in the script.

                int particleSystemBufferSize = 8;
                int particleBufferSize = 65535; // Seems like this is the max buffer size?

                protected ComputeShader shader;

                int[] kernels;

                protected int currentKernel;
                protected string[] kernelNames;

                int sizeOfParticleData;
                int sizeOfParticleSystemData;

                ParticleSystemData[] particleSystemData;
                ParticleData[] particleData;

                // Instead of using particle count, use max particles instead.
                // This way, I can prevent creating new arrays so long as the
                // *potential* number of particles remains the same.

                // This cuts down on GC a lot, even though some setup is required
                // in the editor itself for best results using the GPU particle affectors.

                // In this case, it's best to manually turn down the max particles
                // in the editor to the lowest possible value required to prevent 
                // additional wasted time sending and receiving data from the GPU.

                // Obviously, changes to the particle system list will also cause 
                // issues because the max particle count will immediately change
                // if new ones are added or removed, but at least it's limited to
                // the frame they are added/removed.

                // You'll see in the editor the particle system and particle data buffer 
                // sizes will probably be different based on true or false.

                // For true, the buffer sizes will change in chunks based on the maxParticles
                // of every system, where as otherwise the particle data buffer size will vary
                // from frame to frame as particles are created and killed throughout all the systems
                // the affector has influence over.

                // TL;DR: 

                // If true, then instead of looking at the current particle count, look at the max POTENTIAL
                // particle count (maxParticles from each system) when calculating how large to make the particle
                // buffers to send to the GPU.

                // UPDATE: 

                // After some more testing, it doesn't seem like I get any real benefit from doing this anymore.
                // I think I changed something else in my code that made it more efficient anyways...

                // May as well keep it false now. That way, there's no setup required
                // for the max particle count per system.

                bool onlyUpdateBufferSizesPerSystem = false;

                // =================================	
                // Functions.
                // =================================

                // ...

                protected virtual void Awake()
                {
                    kernelNames = new string[] { "CSMain" };

                    // Make sure shader name matches class name!
                    // Else, this will fail...

                    shader = (ComputeShader)Resources.Load(GetType().Name);
                }

                // ...

                protected virtual void Start()
                {
                    particleSystem = GetComponent<ParticleSystem>();

                    // Prepare local (CPU) buffers.

                    sizeOfParticleSystemData = Marshal.SizeOf(typeof(ParticleSystemData));
                    sizeOfParticleData = Marshal.SizeOf(typeof(ParticleData));

                    particleSystemBufferSize = 0;
                    particleBufferSize = 0;
                }

                // Called once per particle system, before entering second loop for its particles.
                // Used for setting up based on particle system-specific data. 

                protected virtual void perParticleSystemSetup()
                {

                }

                // Override and use this to fill output data.

                protected virtual void setShaderData()
                {

                }

                // ...

                protected virtual void Update()
                {

                }

                // ...

                protected virtual void LateUpdate()
                {
                    float radius = scaledRadius;
                    radiusSqr = radius * radius;

                    deltaTime = Time.deltaTime;

                    // Get all required particle systems.

                    List<ParticleSystem> particleSystems = new List<ParticleSystem>();

                    // LOCAL.
                    // If attached to particle system, use only that.

                    if (this.particleSystem)
                    {
                        particleSystems.Add(particleSystem);
                    }

                    // SELECTIVE.
                    // Else if manually assigned a set of systems, use those.

                    else if (this.particleSystems != null && this.particleSystems.Length > 0)
                    {
                        particleSystems.AddRange(this.particleSystems);
                    }

                    // GLOBAL.
                    // Else, take all the ones from the entire scene.

                    else
                    {
                        particleSystems.AddRange(FindObjectsOfType<ParticleSystem>());
                    }

                    // ...

                    int kernel = shader.FindKernel(kernelNames[currentKernel]);

                    // Create all gradient and curve LUTs.

                    CreateLUT.fromAnimationCurve(LUTSteps, scaleForceByDistance, ref scaleForceByDistanceTexture);

                    // Set global force affector data.

                    setShaderData();

                    // Set shared global force affector data.

                    shader.SetFloat("radiusSqr", radiusSqr);

                    shader.SetFloat("force", force);

                    shader.SetFloat("deltaTime", deltaTime);

                    shader.SetVector("transformPosition", transform.position + offset);
                    shader.SetTexture(currentKernel, "scaleForceByDistance", scaleForceByDistanceTexture);

                    // ...

                    shader.SetInt("LUTSteps", LUTSteps);

                    // Calculate buffer sizes.

                    int previousParticleSystemBufferSize = particleSystemBufferSize;
                    int previousParticleBufferSize = particleBufferSize;

                    particleSystemBufferSize = particleSystems.Count;
                    particleBufferSize = 0;

                    if (onlyUpdateBufferSizesPerSystem)
                    {
                        for (int i = 0; i < particleSystems.Count; i++)
                        { particleBufferSize += particleSystems[i].main.maxParticles; }
                    }
                    else
                    {
                        for (int i = 0; i < particleSystems.Count; i++)
                        { particleBufferSize += particleSystems[i].particleCount; }
                    }

                    // Create buffers.

                    if (particleSystemBufferSize != previousParticleSystemBufferSize || particleSystemData == null)
                    {
                        particleSystemData = new ParticleSystemData[particleSystemBufferSize];
                    }

                    if (particleBufferSize != previousParticleBufferSize || particleData == null)
                    {
                        // If already initialized, release buffers first.

                        particleData = new ParticleData[particleBufferSize];
                    }

                    ComputeBuffer particleSystemBuffer = new ComputeBuffer(particleSystemData.Length, sizeOfParticleSystemData);
                    ComputeBuffer particleBuffer = new ComputeBuffer(particleData.Length, sizeOfParticleData);

                    shader.SetBuffer(kernel, "particleSystemData", particleSystemBuffer);
                    shader.SetBuffer(kernel, "particleData", particleBuffer);

                    // Update.

                    int particleIndexOffset = 0;

                    // Save this array here so it can be reused after the first loop's assignment.
                    // GC is killing FPS more than anything else.

                    ParticleSystem.Particle[][] particlesArray =
                        new ParticleSystem.Particle[particleSystems.Count][];

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        ParticleSystem particleSystem = particleSystems[i];
                        currentParticleSystem = particleSystem;

                        // Get the smaller of the two, since it's possible to change max value and then
                        // the number of particles will exceed the max for a single frame.

                        // May actually be a minor bug in Unity with Shuriken.
                        // I would think the expected behaviour is immediate truncation.
                        // ...meh.

                        int getParticlesMax = Mathf.Min(
                            particleSystem.particleCount, particleSystem.main.maxParticles);

                        particlesArray[i] = new ParticleSystem.Particle[getParticlesMax];

                        ParticleSystem.Particle[] particles = particlesArray[i];
                        int particleCount = particleSystem.GetParticles(particles);

                        // Set particle system data.
                        // Only apply world-space offset if simulating in world space.

                        particleSystemData[i].position = particleSystem.main.simulationSpace ==
                            ParticleSystemSimulationSpace.World ? Vector3.zero : particleSystem.transform.position;

                        particleSystemData[i].externalForcesMultiplier = particleSystem.externalForces.multiplier;

                        particleSystemBuffer.SetData(particleSystemData);

                        // Set particle data.
                        // Continue from index we left off at last.

                        perParticleSystemSetup();

                        for (int j = 0; j < particleCount; j++)
                        {
                            ParticleSystem.Particle particle = particles[j];

                            int index = j + particleIndexOffset;

                            particleData[index].position = particle.position;
                            particleData[index].velocity = particle.velocity;

                            particleData[index].particleSystemIndex = i;
                        }

                        particleIndexOffset += particleCount;
                    }

                    // Pass all that data to the buffer.

                    particleBuffer.SetData(particleData);

                    // Process on GPU.

                    shader.Dispatch(kernel, particleData.Length, 1, 1);

                    // Get results.

                    particleBuffer.GetData(particleData);

                    // Clear these, since we have all we need from them for this frame.

                    particleSystemBuffer.Release();
                    particleBuffer.Release();

                    // Assign new, GPU-processed velocity back to particles in all systems.

                    particleIndexOffset = 0;

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        ParticleSystem particleSystem = particleSystems[i];
                        ParticleSystem.Particle[] particles = particlesArray[i];

                        int particleCount = particleSystem.GetParticles(particles);

                        for (int j = 0; j < particleCount; j++)
                        {
                            particles[j].velocity = particleData[j + particleIndexOffset].velocity;
                        }

                        particleIndexOffset += particleCount;
                        particleSystems[i].SetParticles(particles, particleCount);
                    }
                }

                // ...

                protected virtual void OnDrawGizmosSelected()
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.position + offset, scaledRadius);
                }

                // ...

                void OnDestroy()
                {

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
