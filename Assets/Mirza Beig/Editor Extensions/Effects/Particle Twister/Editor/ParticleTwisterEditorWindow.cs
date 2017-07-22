
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Linq;

using System.Collections;
using System.Collections.Generic;

using MirzaBeig.EditorExtensions.Utilities;

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

            //[CustomEditor(typeof(ParticlePrefab))]
            //[CustomEditor(typeof(Transform))]
            //[CustomEditor(typeof(ParticleSystem))]
            //[CanEditMultipleObjects]

            public class ParticleTwisterEditorWindow : EditorWindow
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // All values in the editor that can be changed
                // by the user and can be restored to default go here.

                public class ParticleTwisterEditorValues
                {
                    // ...

                    public ParticleTwisterEditorValues()
                    {
                        reset();
                    }

                    // ...

                    public void reset()
                    {
                        scale = 50.0f;
                        velocity = 200.0f;
                        resolution = 64;

                        fastForwardTime = 8.0f;
                        fastForwardOnApply = true;

                        autoUpdate = true;

                        invert = false;
                        spiral = true;
                    }

                    // Twister.

                    public float scale;
                    public float velocity;
                    public int resolution; // Keyframe count.

                    // Options.

                    public float fastForwardTime;
                    public bool fastForwardOnApply;

                    public bool autoUpdate;

                    public bool invert;
                    public bool spiral = true;

                }

                // =================================	
                // Variables.
                // =================================

                // Button sizes.

                float applyTwisterButtonHeight = 25.0f;

                // Playback.

                ParticlePlayback particlePlayback = new ParticlePlayback();

                // Selected objects in editor and all the particle systems components.

                List<GameObject> selectedGameObjectsWithParticleSystems = new List<GameObject>();

                // Particle systems and their scale values.

                // I also keep last frame's particle systems because I update
                // the list of particle systems on update. So clearing particles
                // inside the systems may not do anything as the particles are
                // updated and the list set to a length of zero before OnSelectionChange.

                List<ParticleSystem> particleSystems = new List<ParticleSystem>();
                List<ParticleSystem> particleSystemsFromLastFrame = new List<ParticleSystem>();

                // Editor values for applying to particle systems.

                ParticleTwisterEditorValues particleTwisterEditorValues;

                // For labeling and tooltips.

                GUIContent guiContentLabel;

                // =================================	
                // Functions.
                // =================================

                // ...

                [MenuItem("Window/Mirza Beig/Particle Twister")]
                static void showEditor()
                {
                    ParticleTwisterEditorWindow window = GetWindow<ParticleTwisterEditorWindow>(false, "Mirza Beig - Particle Twister");

                    // Static init.

                    // ...

                    // Invoke non-static init.

                    window.initialize();
                }

                // Initialize.

                void initialize()
                {

                }

                // Return an editor window by name.

                public static EditorWindow getEditorWindow(string name)
                {
                    System.Reflection.Assembly assembly = typeof(EditorWindow).Assembly;
                    return GetWindow(assembly.GetType(name));
                }

                // ...

                void OnEnable()
                {
                    particleTwisterEditorValues = new ParticleTwisterEditorValues();
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

                void applyTwisterToAll()
                {
                    float scale = !particleTwisterEditorValues.invert ?
                        particleTwisterEditorValues.scale : -particleTwisterEditorValues.scale;

                    Undo.RecordObjects(particleSystems.ToArray(), "Apply Twister to Particle System(s)");

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].applyTwister(

                            particleTwisterEditorValues.spiral,

                            scale,

                            particleTwisterEditorValues.velocity,
                            particleTwisterEditorValues.resolution);

                        if (particleTwisterEditorValues.fastForwardOnApply)
                        {
                            simulateAndPlay(particleSystems[i]);
                        }
                    }
                }

                // ...

                void simulateAndPlay(ParticleSystem particleSystem)
                {
                    particleSystem.Simulate(
                        particleTwisterEditorValues.fastForwardTime, false, false);

                    particleSystem.Play();
                }

                // ...

                void OnGUI()
                {
                    // Get windows.

                    particlePlayback.updateEditorCameraWindowReferences();

                    // Looks nicer.

                    EditorGUILayout.Separator();

                    // Twister settings.

                    EditorGUILayout.LabelField("- Twister Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    EditorGUI.BeginChangeCheck();
                    {
                        guiContentLabel = new GUIContent("Scale", "Time scale: higher values == more curves / twists.");
                        particleTwisterEditorValues.scale = EditorGUILayout.Slider(guiContentLabel, particleTwisterEditorValues.scale, 1.0f, 100.0f);

                        guiContentLabel = new GUIContent("Velocity", "Velocity scale: higher values == faster particles.");
                        particleTwisterEditorValues.velocity = EditorGUILayout.Slider(guiContentLabel, particleTwisterEditorValues.velocity, 0.25f, 1000.0f);

                        guiContentLabel = new GUIContent("Resolution", "Keyframes used per axis. Higher values == smoother curves. 64 is a decent maximum.");
                        particleTwisterEditorValues.resolution = EditorGUILayout.IntSlider(guiContentLabel, particleTwisterEditorValues.resolution, 8, 1024);
                    }

                    // If any previous GUI elements were modified and set to auto-update, apply twister to all.

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (particleTwisterEditorValues.autoUpdate)
                        {
                            applyTwisterToAll();
                        }
                    }

                    EditorGUILayout.Separator();

                    // Extension options.

                    EditorGUILayout.LabelField("- Twister Options:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    guiContentLabel = new GUIContent("Auto-Update", "Update particles on setting changes.");
                    particleTwisterEditorValues.autoUpdate = EditorGUILayout.Toggle(guiContentLabel, particleTwisterEditorValues.autoUpdate);

                    guiContentLabel = new GUIContent("Auto Fast-Forward", "Simulate particles forward on updates.");
                    particleTwisterEditorValues.fastForwardOnApply = EditorGUILayout.Toggle(guiContentLabel, particleTwisterEditorValues.fastForwardOnApply);

                    EditorGUILayout.Separator();

                    guiContentLabel = new GUIContent("Fast-Forward Time (s)", "Time to simulate particles forward (in seconds).");
                    particleTwisterEditorValues.fastForwardTime = EditorGUILayout.Slider(guiContentLabel, particleTwisterEditorValues.fastForwardTime, 0.5f, 1024.0f);

                    EditorGUILayout.Separator();

                    guiContentLabel = new GUIContent("Invert", "Invert scale factor (reverse direction).");
                    particleTwisterEditorValues.invert = EditorGUILayout.Toggle(guiContentLabel, particleTwisterEditorValues.invert);

                    guiContentLabel = new GUIContent("Spiral", "Spiral velocity curve (else, circular).");
                    particleTwisterEditorValues.spiral = EditorGUILayout.Toggle(guiContentLabel, particleTwisterEditorValues.spiral);

                    EditorGUILayout.Separator();

                    // Buttons to apply.

                    EditorGUILayout.BeginHorizontal();
                    {
                        guiContentLabel = new GUIContent("Apply Twister",
                            "Apply twister settings to all ParticleSystem components in select GameObjects (copy values).");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(applyTwisterButtonHeight)))
                        {
                            applyTwisterToAll();
                        }

                        guiContentLabel = new GUIContent("Fast-Forward",
                            "Simulate particles forward in time.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(applyTwisterButtonHeight)))
                        {
                            for (int i = 0; i < particleSystems.Count; i++)
                            {
                                simulateAndPlay(particleSystems[i]);
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    // Button for creating a new system.

                    guiContentLabel = new GUIContent("Create New",
                        "Create a new default ParticleSystem as a twister.");

                    if (GUILayout.Button(guiContentLabel, GUILayout.Height(applyTwisterButtonHeight)))
                    {
                        GameObject gameObject = new GameObject("Twister", typeof(ParticleSystem));
                        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();

                        particleSystem.setNewParticleTwisterValues();

                        // particleSystem.applyTwister();

                        particleSystem.applyTwister(

                            particleTwisterEditorValues.spiral,
                            particleTwisterEditorValues.scale,
                            particleTwisterEditorValues.velocity,
                            particleTwisterEditorValues.resolution);

                        //simulateAndPlay(particleSystem);
                    }

                    // Button for resetting editor values.

                    guiContentLabel = new GUIContent("Reset Editor",
                        "Resets editor values to their original defaults.");

                    if (GUILayout.Button(guiContentLabel, GUILayout.Height(applyTwisterButtonHeight)))
                    {
                        particleTwisterEditorValues.reset();
                    }

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
