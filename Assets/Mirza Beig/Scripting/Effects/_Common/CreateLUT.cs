
// =================================	
// Namespaces.
// =================================

using UnityEngine;

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
            //[System.Serializable]

            //[RequireComponent(typeof(ParticleSystem))]

            static public class CreateLUT
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

                public static void fromGradient(int steps, Gradient gradient, ref Texture2D texture)
                {
                    if (texture)
                    {
                        MonoBehaviour.Destroy(texture);
                    }

                    texture = new Texture2D(steps, 1);

                    // Assign the first and last beforehand.
                    // Min LUT width resolution is 2, so this will work without problems.

                    texture.SetPixel(0, 0, gradient.Evaluate(0.0f));
                    texture.SetPixel(steps - 1, 0, gradient.Evaluate(1.0f));

                    // Then fill the middle (if any -> meaning if resolution > 2).

                    for (int i = 1; i < steps - 1; i++)
                    {
                        Color colour = gradient.Evaluate(i / (float)steps);
                        texture.SetPixel(i, 0, colour);
                    }

                    texture.Apply();
                }
                public static void fromAnimationCurve(int steps, AnimationCurve curve, ref Texture2D texture)
                {
                    if (texture)
                    {
                        MonoBehaviour.Destroy(texture);
                    }

                    texture = new Texture2D(steps, 1);

                    // Assign the first and last beforehand.
                    // Min LUT width resolution is 2, so this will work without problems.

                    texture.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, curve.Evaluate(0.0f)));
                    texture.SetPixel(steps - 1, 0, new Color(0.0f, 0.0f, 0.0f, curve.Evaluate(1.0f)));

                    // Then fill the middle (if any -> meaning if resolution > 2).

                    for (int i = 1; i < steps - 1; i++)
                    {
                        float value = curve.Evaluate(i / (float)steps);
                        texture.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, value));
                    }

                    texture.Apply();
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
