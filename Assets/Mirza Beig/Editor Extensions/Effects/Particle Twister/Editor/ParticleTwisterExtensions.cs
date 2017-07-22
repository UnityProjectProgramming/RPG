
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Collections;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace EditorExtensions
    {

        namespace Effects
        {

            // =================================	
            // Classes.
            // =================================

            public static class ParticleTwisterExtensions
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

                // ...

                public static void applyTwister(this ParticleSystem particleSystem, bool spiral,
                    float scale = 50.0f, float velocity = 200.0f, int resolution = 64)
                {
                    // Get as serialized object. Allows undo/redo.

                    SerializedObject particleSystemObject = new SerializedObject(particleSystem);

                    // Velocity curve spiral or circle.

                    Keyframe[] keyframesX = new Keyframe[resolution];
                    Keyframe[] keyframesY = new Keyframe[resolution];

                    float resolutionStep = 1.0f / resolution;

                    // Spiral.

                    if (spiral)
                    {
                        for (int i = 0; i < resolution; i++)
                        {
                            float time = resolutionStep * i;

                            float x = time * Mathf.Cos(time * scale);
                            float y = time * Mathf.Sin(time * scale);

                            keyframesX[i] = new Keyframe(time, x);
                            keyframesY[i] = new Keyframe(time, y);
                        }
                    }

                    // Circle.

                    else
                    {
                        for (int i = 0; i < resolution; i++)
                        {
                            float time = resolutionStep * i;

                            float x = Mathf.Cos(time * scale);
                            float y = Mathf.Sin(time * scale);

                            keyframesX[i] = new Keyframe(time, x);
                            keyframesY[i] = new Keyframe(time, y);
                        }
                    }

                    particleSystemObject.FindProperty("VelocityModule.x.minMaxState").intValue = 1; // Curves.
                    particleSystemObject.FindProperty("VelocityModule.y.minMaxState").intValue = 1; // Curves.
                    particleSystemObject.FindProperty("VelocityModule.z.minMaxState").intValue = 1; // Curves.

                    particleSystemObject.FindProperty("VelocityModule.x.maxCurve").animationCurveValue = new AnimationCurve(keyframesX);
                    particleSystemObject.FindProperty("VelocityModule.z.maxCurve").animationCurveValue = new AnimationCurve(keyframesY);

                    particleSystemObject.FindProperty("VelocityModule.x.scalar").floatValue = velocity;
                    particleSystemObject.FindProperty("VelocityModule.z.scalar").floatValue = velocity;

                    particleSystemObject.ApplyModifiedProperties();
                }

                // ...

                public static void setNewParticleTwisterValues(this ParticleSystem particleSystem)
                {
                    // Get as serialized object. Allows undo/redo.

                    SerializedObject particleSystemObject = new SerializedObject(particleSystem);

                    // Defaults for a new system.

                    particleSystemObject.FindProperty("InitialModule.startSpeed.scalar").floatValue = 0.0f;

                    particleSystemObject.FindProperty("ForceModule.enabled").boolValue = true;
                    particleSystemObject.FindProperty("ForceModule.y.scalar").floatValue = 5.0f;

                    particleSystemObject.FindProperty("ShapeModule.type").intValue = 0; // Sphere emitter.
                    particleSystemObject.FindProperty("ShapeModule.radius").floatValue = 0.0f;

                    particleSystemObject.ApplyModifiedProperties();

                    // Reset transform.

                    SerializedObject transformObject = new SerializedObject(particleSystem.transform);

                    transformObject.FindProperty("m_LocalPosition").vector3Value = Vector3.zero;
                    transformObject.FindProperty("m_LocalRotation").quaternionValue = Quaternion.identity;
                    transformObject.FindProperty("m_LocalScale").vector3Value = new Vector3(1.0f, 1.0f, 1.0f);

                    transformObject.ApplyModifiedProperties();
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
