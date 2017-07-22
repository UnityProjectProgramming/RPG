
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

            public abstract class ParticleAffector : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                protected struct GetForceParameters
                {
                    public float distanceToAffectorCenterSqr;

                    public Vector3 scaledDirectionToAffectorCenter;
                    public Vector3 particlePosition;
                }

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("Common Controls")]

                public float radius = Mathf.Infinity;
                public float force = 5.0f;

                float deltaTime;

                public Vector3 offset = Vector3.zero;

                // If true, set limit velocity directly from the affector.

                // TO-DO: Find out how Shuriken implements frame-independent 
                // velocity damping.

                //[Range(0.0f, 1.0f)]
                //public float dampen = 0.0f;
                //public bool overrideShurikenDampen = false;

                public float scaledRadius
                {
                    get
                    {
                        return radius * transform.lossyScale.x;
                    }
                }

                public AnimationCurve scaleForceByDistance = new AnimationCurve(

                        new Keyframe(0.0f, 1.0f),
                        new Keyframe(1.0f, 1.0f)

                    );

                // If (attached to a particle system): forces will be LOCAL.
                // Else if (attached to a particle system): forces will be SELECTIVE.
                // Else: forces will be GLOBAL.

                new ParticleSystem particleSystem;
                public ParticleSystem[] particleSystems;

                // Current iteration of the particle systems when looping through all of them.
                // Useful to derived classes (like for the vortex particle affector).

                protected ParticleSystem currentParticleSystem;

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
                    particleSystem = GetComponent<ParticleSystem>();
                }

                // Called once per particle system, before entering second loop for its particles.
                // Used for setting up based on particle system-specific data. 

                protected virtual void perParticleSystemSetup()
                {

                }

                // Direction is NOT normalized.

                protected virtual Vector3 getForce(GetForceParameters parameters)
                {
                    return Vector3.zero;
                }

                // ...

                protected virtual void Update()
                {

                }

                // ...

                protected virtual void LateUpdate()
                {
                    float radius = scaledRadius;
                    float radiusSqr = radius * radius;

                    deltaTime = Time.deltaTime;

                    Vector3 transformPosition = transform.position + offset;

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

                    GetForceParameters parameters = new GetForceParameters();

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        ParticleSystem particleSystem = particleSystems[i];
                        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];

                        currentParticleSystem = particleSystem;
                        Transform currentParticleSystemTransform = particleSystem.transform;

                        // Only apply world-space offset if simulating in local space (else, it's automatically there).

                        //Vector3 particleSystemPosition = particleSystem.main.simulationSpace ==
                        //    ParticleSystemSimulationSpace.World ? Vector3.zero : particleSystem.transform.position;

                        perParticleSystemSetup();

                        // If true, set limit velocity directly from the affector.

                        //if (overrideShurikenDampen)
                        //{
                        //    ParticleSystem.LimitVelocityOverLifetimeModule lv = particleSystem.limitVelocityOverLifetime;

                        //    lv.enabled = true;
                        //    lv.dampen = dampen;
                        //}

                        int particleCount = particleSystem.GetParticles(particles);

                        for (int j = 0; j < particleCount; j++)
                        {
                            ParticleSystem.Particle particle = particles[j];

                            parameters.particlePosition = particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World ?

                                particle.position :
                                currentParticleSystemTransform.TransformPoint(particle.position);

                            parameters.scaledDirectionToAffectorCenter = parameters.particlePosition - transformPosition;
                            parameters.distanceToAffectorCenterSqr = parameters.scaledDirectionToAffectorCenter.sqrMagnitude;

                            //Debug.DrawLine(parameters.particlePosition, transformPosition);

                            if (parameters.distanceToAffectorCenterSqr < radiusSqr)
                            {
                                // 0.0f -> 0.99...f;

                                float distanceToCenterNormalized = parameters.distanceToAffectorCenterSqr / radiusSqr;

                                // Evaluating a curve within a loop which is very likely to exceed a few thousand
                                // iterations produces a noticeable FPS drop (around minus 2 - 5). Might be a worthwhile
                                // optimization to check outside all loops if the curve is constant (all keyframes same value),
                                // and then run a different block of code if true that uses that value as a stored float without 
                                // having to call Evaluate(t).

                                float distanceScale = scaleForceByDistance.Evaluate(distanceToCenterNormalized);

                                Vector3 force = ((getForce(parameters) * this.force) * distanceScale) * particleSystem.externalForces.multiplier;

                                if (particleSystem.main.simulationSpace != ParticleSystemSimulationSpace.World)
                                {
                                    force = currentParticleSystemTransform.InverseTransformVector(force);
                                }

                                force *= deltaTime;
                                particle.velocity += force;

                                particles[j] = particle;
                            }
                        }

                        particleSystems[i].SetParticles(particles, particleCount);
                    }
                }

                // ...

                protected virtual void OnDrawGizmosSelected()
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.position + offset, scaledRadius);
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
