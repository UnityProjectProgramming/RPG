
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

            public class ParticleEditorUtility
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

                // Clear all active particles.

                public static void clearParticles(List<ParticleSystem> particleSystems)
                {
                    for (int i = 0; i < particleSystems.Count; i++)
                    {
                        if (particleSystems[i])
                        {
                            particleSystems[i].Clear(false);
                        }
                    }
                }

                // Get a list of particle system components from all selected GameObjects as well as the gameObjects themselves.

                public static void getSelectedParticleSystems(ref List<ParticleSystem> particleSystems, ref List<GameObject> selectedGameObjectsWithParticleSystems)
                {
                    // Clear for updated selection.

                    particleSystems.Clear();

                    // NOTE: These do the same thing, but I'm not sure which one is better/faster/more efficient.
                    // I _think_ the second method is faster because when I select a GO in the hierarchy with a large
                    // number of embedded particles, it seems to cause the least delay.

                    // SelectedGameObjects == all objects which either have a particle system on them, or in at least one child.

                    //selectedGameObjectsWithParticleSystems = Selection.gameObjects.Where(
                    //    selected => selected.GetComponentsInChildren<ParticleSystem>(true) != null).ToList();

                    //// All the particle systems in all selected.

                    //for (int i = 0; i < selectedGameObjectsWithParticleSystems.Count; i++)
                    //{
                    //    // Merge to self while ignoring duplicate components.

                    //    particleSystems = particleSystems.Union(selectedGameObjectsWithParticleSystems[i].GetComponentsInChildren<ParticleSystem>(true)).ToList();
                    //}

                    // Allows selection of inactive objects.

                    // In this version, the parameters can be lists rather than arrays so I can
                    // just use Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel) as GameObject[]
                    // as rather than running a loop to add converted elements.

                    Object[] selectedObjects =
                        Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);

                    selectedGameObjectsWithParticleSystems.Clear();

                    for (int i = 0; i < selectedObjects.Length; i++)
                    {
                        selectedGameObjectsWithParticleSystems.Add(selectedObjects[i] as GameObject);
                    }

                    for (int i = 0; i < selectedGameObjectsWithParticleSystems.Count; i++)
                    {
                        // Merge to self while ignoring duplicate components.

                        particleSystems.AddRange(selectedGameObjectsWithParticleSystems[i].GetComponentsInChildren<ParticleSystem>(true));
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
