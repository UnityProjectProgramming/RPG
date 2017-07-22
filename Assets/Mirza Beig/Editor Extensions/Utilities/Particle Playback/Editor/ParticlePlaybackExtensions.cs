
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

        namespace Utilities
        {

            // =================================	
            // Classes.
            // =================================

            public static class ParticlePlaybackExtensions
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

                // Stops the system, then re-simulates to same playback time before stop.

                // Note that time has a range of 0.0f to the duration set in the ParticleSystem component.
                // It will restart when it hits duration automatically, and I can't think of an easy way
                // around that. So for systems that don't loop, this may cause problems with some particles
                // not being emitted when scrubbing through, even though this method allows for continuous
                // playback since it resumes from the last position. 

                // In addition, you'll also notice jumps every n-seconds, where n is the duration.
                // Since the internal emitter time is reset... so it's kinda-sorta SUPER annoying.

                public static void restartToCurrentTime(this ParticleSystem particleSystem)
                {
                    // Save time.

                    float time = particleSystem.time;

                    // Restart (without actually setting restart to true in last Simulate() parameter).

                    particleSystem.Stop(false);
                    particleSystem.Clear(false);

                    particleSystem.Play(false);

                    particleSystem.Simulate(time, false, false);
                }

                // Stops the system, then re-simulates to given time.

                public static void setPlaybackPosition(this ParticleSystem particleSystem, float time)
                {
                    particleSystem.Stop(false);
                    particleSystem.Clear(false);

                    particleSystem.Play(false);

                    particleSystem.Simulate(time, false, false);
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
