
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

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

            public class ParticlePlayback
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                public enum ParticlePlaybackState
                {
                    NULL,

                    Playing, Paused, Stopped
                }

                // =================================	
                // Variables.
                // =================================

                // Button sizes.

                public float playbackButtonHeight = 25.0f;

                // Selected objects in editor and all the particle systems components.

                //List<ParticleSystem> particleSystems = new List<ParticleSystem>();

                // Playback.

                public float particlePlaybackPosition = 0.0f;
                public float particlePlaybackPositionSliderMax = 2.0f;

                // Sets the entire particle system set to a single randomized value.

                bool randomizeParticleSeed = false;

                // Sets different random seeds for each particle system.

                bool randomizeMultiLevelParticleSeed = false;

                int particleRandomSeed = 0;

                // Because of interval jumps when adjusting the slider,
                // the value Unity applies to the slider tends to overflow
                // the actual maximum and resets itself. 

                // Simple solution: I limit the max to half.

                // Downside? It's only HALF as random as before... which doesn't even make a difference.

                // EDIT: There's another issue where calling inSlider with a large value
                // causes it to change slightly when passed through itself.

                // The issue was apparent when using the randomize button.
                // Setting it to max / 32 seems to solve the problem...

                const int intSliderMaxValue = int.MaxValue / 32;

                // Clear all particles on screen from selection when selection changes?

                public bool clearParticlesOnSelectionChange = false;

                // Button pressed state.

                ParticlePlaybackState particlePlaybackState = ParticlePlaybackState.NULL;

                // For calculating deltaTime in Update().

                float lastUpdateTime = 0.0f;

                // Active editor windows.

                EditorWindow gameView;
                EditorWindow sceneView;

                // Scrolling view position for selected objects.

                Vector2 scrollPosition;

                // For labeling and tooltips.

                GUIContent guiContentLabel;

                // =================================	
                // Functions.
                // =================================

                // ...

                // Return an editor window by name.

                public EditorWindow getEditorWindow(string name)
                {
                    System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
                    Object[] editorWindows = Resources.FindObjectsOfTypeAll(assembly.GetType(name));

                    // If window doesn't exist, GetWindow() creates it. 
                    // This is not what I want. So if the window doesn't exist, skip this.

                    if (editorWindows.Length != 0)
                    {
                        return EditorWindow.GetWindow(assembly.GetType(name), false, null, false);
                    }

                    return null;
                }

                // Update/refresh/repaint all NON-EDITOR windows.

                public void repaintEditorCameraWindows()
                {
                    if (gameView)
                    {
                        gameView.Repaint();
                    }
                    if (sceneView)
                    {
                        sceneView.Repaint();
                    }
                }

                // ...

                public void updateEditorCameraWindowReferences()
                {
                    gameView = getEditorWindow("UnityEditor.GameView");
                    sceneView = getEditorWindow("UnityEditor.SceneView");
                }

                // ...

                public void pause(List<ParticleSystem> particleSystems)
                {
                    lastUpdateTime = 0.0f;

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].Pause(false);
                    }
                }

                // Simulates full-circle back to current position.

                public void loopback(ParticleSystem particleSystem)
                {
                    // Lock random seed.

                    bool autoRandomSeed = particleSystem.useAutoRandomSeed;

                    // Save state before set.

                    ParticlePlaybackState state;

                    if (particleSystem.isPlaying)
                    {
                        state = ParticlePlaybackState.Playing;
                    }
                    else if (particleSystem.isPaused)
                    {
                        state = ParticlePlaybackState.Paused;
                    }
                    else
                    {
                        state = ParticlePlaybackState.Stopped;
                    }

                    // DON'T RESTART the simulation.
                    // Instead, stop, play, then simulate to time.

                    // Keep last false in Simulate to prevent restarts.
                    // Else, particles will pop in and out...

                    // Also requires clear in that case.

                    particleSystem.Stop(false);
                    particleSystem.Clear(false);

                    particleSystem.randomSeed = (uint)particleRandomSeed;

                    particleSystem.Play(false);

                    particleSystem.Simulate(particlePlaybackPosition, false, false);

                    // Resume from saved playback state.

                    particleSystem.Pause();
                    particleSystem.useAutoRandomSeed = autoRandomSeed;

                    switch (state)
                    {
                        case ParticlePlaybackState.Playing:
                            {
                                particleSystem.Play(false);

                                break;
                            }
                        case ParticlePlaybackState.Paused:
                            {
                                particleSystem.Pause(false);

                                break;
                            }
                        case ParticlePlaybackState.Stopped:
                            {
                                particleSystem.Stop(false);

                                break;
                            }
                    }
                }

                // ...

                public void GUIPlaybackSettings(List<ParticleSystem> particleSystems)
                {
                    // Get windows.

                    updateEditorCameraWindowReferences();

                    // Playback settings.

                    EditorGUILayout.LabelField("- Playback Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    // Timer options.

                    GUI.enabled = particleSystems.Count != 0;

                    EditorGUI.BeginChangeCheck();
                    {
                        guiContentLabel = new GUIContent("Playback Position (s)", "Playback position of all selected particle systems in seconds.");
                        particlePlaybackPosition = EditorGUILayout.Slider(guiContentLabel, particlePlaybackPosition, 0.0f, particlePlaybackPositionSliderMax);
                    }

                    // Only update if slider changed.

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Set playback position.

                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            loopback(particleSystems[i]);
                        }

                        // Refresh.

                        repaintEditorCameraWindows();
                    }

                    GUI.enabled = true;

                    EditorGUILayout.Separator();

                    guiContentLabel = new GUIContent("Playback Position Max (s)", "Playback position maximum value in seconds.");
                    particlePlaybackPositionSliderMax = EditorGUILayout.Slider(guiContentLabel, particlePlaybackPositionSliderMax, 0.0f, 32.0f);

                    EditorGUILayout.Separator();

                    // Playback buttons.

                    GUI.enabled = particleSystems.Count != 0;

                    EditorGUILayout.BeginHorizontal();
                    {
                        guiContentLabel = new GUIContent("Play",
                            "Play all selected particles.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(playbackButtonHeight)))
                        {
                            if (randomizeParticleSeed)
                            {
                                particleRandomSeed = Random.Range(0, intSliderMaxValue);
                            }

                            for (int i = 0; i < particleSystems.Count; i++)
                            {
                                if (randomizeParticleSeed && randomizeMultiLevelParticleSeed)
                                {
                                    particleRandomSeed = Random.Range(0, intSliderMaxValue);
                                }

                                bool autoRandomSeed = particleSystems[i].useAutoRandomSeed;

                                particleSystems[i].randomSeed = (uint)particleRandomSeed;

                                particleSystems[i].useAutoRandomSeed = autoRandomSeed;

                                if (particlePlaybackState != ParticlePlaybackState.Paused)
                                {
                                    particleSystems[i].Stop(false);
                                    particleSystems[i].Clear(false);
                                }

                                particleSystems[i].Play(false);
                            }

                            particlePlaybackState = ParticlePlaybackState.Playing;
                        }
                        guiContentLabel = new GUIContent("Pause",
                            "Pause all selected particles.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(playbackButtonHeight)))
                        {
                            for (int i = 0; i < particleSystems.Count; i++)
                            {
                                particleSystems[i].Pause(false);

                                lastUpdateTime = 0.0f;
                            }

                            particlePlaybackState = ParticlePlaybackState.Paused;
                        }
                        guiContentLabel = new GUIContent("Stop",
                            "Stop all selected particles.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(playbackButtonHeight)))
                        {
                            for (int i = 0; i < particleSystems.Count; i++)
                            {
                                particleSystems[i].Stop(false);
                                particleSystems[i].Clear(false);

                                lastUpdateTime = 0.0f;
                            }

                            particlePlaybackState = ParticlePlaybackState.Stopped;
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                    //EditorGUILayout.Separator();

                    // Button to clear all particles.

                    guiContentLabel = new GUIContent("Clear",
                        "Clear all particles on screen.");

                    if (GUILayout.Button(guiContentLabel, GUILayout.Height(playbackButtonHeight)))
                    {
                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            particleSystems[i].Clear(false);
                            particleSystems[i].Stop();
                        }

                        repaintEditorCameraWindows();
                    }

                    GUI.enabled = true;

                    EditorGUILayout.Separator();

                    // Display playback state.

                    //EditorGUILayout.HelpBox("> " + particlePlaybackState.ToString(), MessageType.None);
                    //EditorGUILayout.Separator();

                    // Particle seed options.

                    guiContentLabel = new GUIContent("Randomize Seed", "Set all particle systems' seed to a single random value.");
                    randomizeParticleSeed = EditorGUILayout.Toggle(guiContentLabel, randomizeParticleSeed);

                    GUI.enabled = randomizeParticleSeed;

                    guiContentLabel = new GUIContent("Multi-Level Random Seed", "Randomize particle seed per system.");
                    randomizeMultiLevelParticleSeed = EditorGUILayout.Toggle(guiContentLabel, randomizeMultiLevelParticleSeed);

                    GUI.enabled = !randomizeParticleSeed;

                    EditorGUILayout.Separator();
                    EditorGUI.BeginChangeCheck();
                    {
                        guiContentLabel = new GUIContent("Particle Random Seed", "Set the \"random\" particle seed. 0 == auto-randomize on awake.");
                        particleRandomSeed = EditorGUILayout.IntSlider(guiContentLabel, particleRandomSeed, 0, intSliderMaxValue);
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            loopback(particleSystems[i]);
                        }
                    }

                    // Randomize seed.

                    guiContentLabel = new GUIContent("Randomize Seed",
                        "Generate a random seed value for the slider.");

                    if (GUILayout.Button(guiContentLabel, GUILayout.Height(playbackButtonHeight)))
                    {
                        particleRandomSeed = Random.Range(0, intSliderMaxValue);

                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            loopback(particleSystems[i]);
                        }
                    }

                    GUI.enabled = true;

                    EditorGUILayout.Separator();

                    // Clear emitted particles on-screen from selection on selection change?

                    guiContentLabel = new GUIContent("Auto-Clear Particles", "Clear all emitted particles from previous selection on selection change.");
                    clearParticlesOnSelectionChange = EditorGUILayout.Toggle(guiContentLabel, clearParticlesOnSelectionChange);
                }

                // ...

                public void GUIParticleSelection(List<GameObject> selectedGameObjectsWithParticleSystems)
                {
                    EditorGUILayout.Separator();

                    // Selected objects.

                    guiContentLabel = new GUIContent("- Selected:",
                        "Selected GameObjects must be active and contain at least one active ParticleSystem component in their hierarchy.");

                    EditorGUILayout.LabelField(guiContentLabel, EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                    {
                        if (selectedGameObjectsWithParticleSystems.Count == 0)
                        {
                            EditorGUILayout.LabelField("Please select GameObjects with at least one ParticleSystem component.", EditorStyles.miniBoldLabel);
                        }
                        else
                        {
                            // Selection will be ordered based on scene hierarchy.
                            // Reverse here, else they will be listed in reverse.

                            for (int i = selectedGameObjectsWithParticleSystems.Count - 1; i != -1; i--)
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField(">   " + selectedGameObjectsWithParticleSystems[i].name, EditorStyles.miniLabel);
                                }

                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }

                    EditorGUILayout.EndScrollView();
                }

                // ...

                public void update(List<ParticleSystem> particleSystems)
                {
                    if (particleSystems.Count != 0 &&
                        particlePlaybackState == ParticlePlaybackState.Playing)
                    {
                        // Get windows.

                        //updateEditorCameraWindowReferences();

                        // Calculate time passed since last call to function.

                        float deltaTime = Time.realtimeSinceStartup - lastUpdateTime;

                        // Force game view to refresh if not in play mode 
                        // so ALL particles will play (and not just selected ones).

                        if (!Application.isPlaying && lastUpdateTime != 0.0f)
                        {
                            for (int i = 0; i < particleSystems.Count; i++)
                            {
                                particleSystems[i].Simulate(deltaTime, false, false);
                            }

                            // Update slider.

                            //particlePlaybackPosition += deltaTime;

                            // Update window displays.

                            repaintEditorCameraWindows();

                            //switch (particlePlaybackState)
                            //{
                            //    case ParticlePlaybackState.Playing:
                            //        {
                            //            for (int i = 0; i < particleSystems.Count; i++)
                            //            {
                            //                particleSystems[i].Simulate(1.0f / 60.0f, false, false);
                            //            }

                            //         repaintCameraViews();

                            //            break;
                            //        }
                            //    case ParticlePlaybackState.Paused:
                            //        {

                            //            break;
                            //        }
                            //    default:
                            //        {

                            //            break;
                            //        }
                            //}
                        }

                        // Set to time at end of function.

                        lastUpdateTime = Time.realtimeSinceStartup;
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
