
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace EditorExtensions
    {

        namespace Utilities
        {

            // =================================	
            // Classes.
            // =================================

            public static class ParticleScalerExtensions
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

                // Scales against self.

                public static void scale(this ParticleSystem particleSystem, float scale, bool scaleTransformLocalPosition)
                {
                    // Transform.

                    if (scaleTransformLocalPosition)
                    {
                        particleSystem.transform.localPosition *= scale;
                    }

                    // Particle system.

                    ParticleSystem.MainModule main = particleSystem.main;
                    ParticleSystem.ShapeModule shape = particleSystem.shape;
                    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = particleSystem.velocityOverLifetime;
                    ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = particleSystem.limitVelocityOverLifetime;
                    ParticleSystem.ForceOverLifetimeModule forceOverLifetime = particleSystem.forceOverLifetime;

                    ParticleSystem.MinMaxCurve mmCurve;

                    // Main.

                    mmCurve = main.startSpeed;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    main.startSpeed = mmCurve;

                    mmCurve = main.startSize;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    main.startSize = mmCurve;

                    // Shape.

                    shape.radius *= scale;
                    shape.scale *= scale;
                    shape.angle *= scale;
                    shape.randomDirectionAmount *= scale;
                    shape.sphericalDirectionAmount *= scale;
                    shape.meshScale *= scale;
                    shape.normalOffset *= scale;

                    // Velocity over lifetime.

                    mmCurve = velocityOverLifetime.x;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    velocityOverLifetime.x = mmCurve;

                    mmCurve = velocityOverLifetime.y;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    velocityOverLifetime.y = mmCurve;

                    mmCurve = velocityOverLifetime.z;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    velocityOverLifetime.z = mmCurve;

                    // Force over lifetime.

                    mmCurve = forceOverLifetime.x;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    forceOverLifetime.x = mmCurve;

                    mmCurve = forceOverLifetime.y;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    forceOverLifetime.y = mmCurve;

                    mmCurve = forceOverLifetime.z;
                    mmCurve.constantMin *= scale;
                    mmCurve.constantMax *= scale;
                    forceOverLifetime.z = mmCurve;
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
