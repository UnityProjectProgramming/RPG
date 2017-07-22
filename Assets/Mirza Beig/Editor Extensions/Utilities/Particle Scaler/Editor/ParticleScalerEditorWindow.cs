
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Reflection;

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

            //[CustomEditor(typeof(ParticlePrefab))]
            //[CustomEditor(typeof(Transform))]
            //[CustomEditor(typeof(ParticleSystem))]
            //[CanEditMultipleObjects]

            public class ParticleScalerEditorWindow : EditorWindow
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // Scale to apply.

                float customScale = 1.0f;

                // Also scale transform local position?

                bool scaleTransformLocalPosition = true;

                // Button sizes.

                float scalePresetButtonHeight = 25.0f;
                float modulePresetButtonHeight = 25.0f;

                float applyCustomScaleButtonHeight = 35.0f;

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

                // For labeling and tooltips.

                GUIContent guiContentLabel;

                // =================================	
                // Functions.
                // =================================

                // ...

                [MenuItem("Window/Mirza Beig/Particle Scaler")]
                static void showEditor()
                {
                    ParticleScalerEditorWindow window =
                        EditorWindow.GetWindow<ParticleScalerEditorWindow>(false, "Mirza Beig - Particle Scaler");

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

                void applyScaleToAll(float scale)
                {
                    Undo.RecordObjects(particleSystems.ToArray().Select(x => x.transform).ToArray(), "Scale Transform(s)");
                    saveUndo();

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        particleSystems[i].scale(scale, scaleTransformLocalPosition);
                        particlePlayback.loopback(particleSystems[i]);
                    }
                }

                // ...

                void saveUndo()
                {
                    Undo.RecordObjects(particleSystems.ToArray(), "Scale Particle System(s)");
                }

                // ...

                void setScalingModeForAll(ParticleSystemScalingMode mode)
                {
                    saveUndo();

                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        ParticleSystem.MainModule m = particleSystems[i].main;
                        m.scalingMode = mode;
                    }
                }

                // ...

                void OnGUI()
                {
                    // Get windows.

                    particlePlayback.updateEditorCameraWindowReferences();

                    // Looks nicer.

                    EditorGUILayout.Separator();

                    // Scale settings.

                    EditorGUILayout.LabelField("- Scale Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    // Extension options.

                    guiContentLabel = new GUIContent("Scale Transform Position", "Scale local position in Transform component.");
                    scaleTransformLocalPosition = EditorGUILayout.Toggle(guiContentLabel, scaleTransformLocalPosition);

                    EditorGUILayout.Separator();

                    guiContentLabel = new GUIContent("Custom Scale", "ParticleSystem component scale factor.");
                    customScale = EditorGUILayout.Slider(guiContentLabel, customScale, 0.5f, 2.0f);

                    EditorGUILayout.Separator();

                    // Button to apply custom scale.

                    guiContentLabel = new GUIContent("Apply Custom Scale",
                        "Apply custom scaling factor to all ParticleSystem components in select GameObjects.");

                    if (GUILayout.Button(guiContentLabel, GUILayout.Height(applyCustomScaleButtonHeight)))
                    {
                        applyScaleToAll(customScale);
                    }

                    //EditorGUILayout.Separator();
                    GUI.enabled = particleSystems.Count != 0;

                    // Buttons for quick-apply scale factor presets.

                    EditorGUILayout.BeginHorizontal();
                    {
                        guiContentLabel = new GUIContent("Scale x0.5",
                            "Apply a preset scale factor of x0.5.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(0.5f);
                        }

                        guiContentLabel = new GUIContent("Scale x0.75",
                            "Apply a preset scale factor of x0.75.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(0.75f);
                        }

                        guiContentLabel = new GUIContent("Scale x1.5",
                            "Apply a preset scale factor of x1.5.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(1.5f);
                        }

                        guiContentLabel = new GUIContent("Scale x2.0",
                            "Apply a preset scale factor of x2.0.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(scalePresetButtonHeight)))
                        {
                            applyScaleToAll(2.0f);
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();

                    GUI.enabled = true;

                    EditorGUILayout.Separator();

                    // Module settings.

                    EditorGUILayout.LabelField("- Scale Mode Settings:", EditorStyles.boldLabel);
                    EditorGUILayout.Separator();

                    // Buttons for quick-apply scale factor presets.

                    EditorGUILayout.BeginHorizontal();
                    {
                        guiContentLabel = new GUIContent("Hierarchy",
                            "Change all particle systems to use \"Hierarchy\" as the scaling mode.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(modulePresetButtonHeight)))
                        {
                            setScalingModeForAll(ParticleSystemScalingMode.Hierarchy);
                        }

                        guiContentLabel = new GUIContent("Local",
                            "Change all particle systems to use \"Local\" as the scaling mode.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(modulePresetButtonHeight)))
                        {
                            setScalingModeForAll(ParticleSystemScalingMode.Local);
                        }

                        guiContentLabel = new GUIContent("Shape",
                            "Change all particle systems to use \"Shape\" as the scaling mode.");

                        if (GUILayout.Button(guiContentLabel, GUILayout.Height(modulePresetButtonHeight)))
                        {
                            setScalingModeForAll(ParticleSystemScalingMode.Shape);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

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
                    // (Re-)verify current list of particles.

                    ParticleEditorUtility.getSelectedParticleSystems(
                        ref particleSystems, ref selectedGameObjectsWithParticleSystems);
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
