
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Linq;

using System.Collections;
using System.Collections.Generic;

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

            //[ExecuteInEditMode]

            //[System.Serializable]
            //[CustomEditor(typeof(ShurikenSpritesheet))]

            public class ParticlePlaybackEditorWindow : EditorWindow
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                ParticlePlayback particlePlayback = new ParticlePlayback();

                // Selected objects in editor and all the particle systems components.

                List<GameObject> selectedGameObjectsWithParticleSystems = new List<GameObject>();

                // I also keep last frame's particle systems because I update
                // the list of particle systems on update. So clearing particles
                // inside the systems may not do anything as the particles are
                // updated and the list set to a length of zero before OnSelectionChange.

                List<ParticleSystem> particleSystems = new List<ParticleSystem>();
                List<ParticleSystem> particleSystemsFromLastFrame = new List<ParticleSystem>();

                // =================================	
                // Functions.
                // =================================

                // Create.

                [MenuItem("Window/Mirza Beig/Particle Playback")]
                static void showEditor()
                {
                    // Get window reference.

                    ParticlePlaybackEditorWindow window =
                        EditorWindow.GetWindow<ParticlePlaybackEditorWindow>(false, "Mirza Beig - Particle Playback");

                    // Static init.

                    // ...

                    // Invoke non-static init.

                    window.initialize();

                    // Do a first check.

                    window.OnSelectionChange();
                }

                // Initialize.

                void initialize()
                {

                }

                // ...

                void OnSelectionChange()
                {
                    // Clear if set to clear on selection change.

                    if (particlePlayback.clearParticlesOnSelectionChange)
                    {
                        ParticleEditorUtility.clearParticles(particleSystems);
                        ParticleEditorUtility.clearParticles(particleSystemsFromLastFrame);

                        particlePlayback.repaintEditorCameraWindows();
                    }

                    // Pause all selected particles.

                    else if (!Application.isPlaying)
                    {
                        particlePlayback.pause(particleSystems);
                    }

                    // (Re-)verify current list of particles.

                    ParticleEditorUtility.getSelectedParticleSystems(ref particleSystems, ref selectedGameObjectsWithParticleSystems);
                }

                // ...

                void OnGUI()
                {
                    // Looks nicer.

                    EditorGUILayout.Separator();

                    // Playback settings.

                    particlePlayback.GUIPlaybackSettings(particleSystems);
                    EditorGUILayout.Separator();

                    // Selected objects.

                    particlePlayback.GUIParticleSelection(selectedGameObjectsWithParticleSystems);
                }

                // ...

                void OnInspectorUpdate()
                {
                    Repaint();
                }

                // ...

                void Update()
                {
                    // (Re-)verify current list of particles.

                    particleSystemsFromLastFrame =
                        new List<ParticleSystem>(particleSystems);

                    ParticleEditorUtility.getSelectedParticleSystems(
                        ref particleSystems, ref selectedGameObjectsWithParticleSystems);

                    particlePlayback.update(particleSystems);
                }

                // ...

                void OnFocus()
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
