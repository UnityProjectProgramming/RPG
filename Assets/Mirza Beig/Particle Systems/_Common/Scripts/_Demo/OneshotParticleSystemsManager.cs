
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            public class OneshotParticleSystemsManager : ParticleManager
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                public LayerMask mouseRaycastLayerMask;
                List<ParticleSystems> spawnedPrefabs;

                // Don't allow spawning if true.
                // Used for button clicks vs. empty-space clicks.

                public bool disableSpawn { get; set; }

                // =================================	
                // Functions.
                // =================================

                // ...

                protected override void Awake()
                {
                    base.Awake();
                }

                // ...

                protected override void Start()
                {
                    base.Start();

                    // ...

                    disableSpawn = false;
                    spawnedPrefabs = new List<ParticleSystems>();
                }

                // Get rid of spawned systems when re-activated.

                void OnEnable()
                {
                    //clear();
                }

                // ...

                public void clear()
                {
                    if (spawnedPrefabs != null)
                    {
                        for (int i = 0; i < spawnedPrefabs.Count; i++)
                        {
                            if (spawnedPrefabs[i])
                            {
                                Destroy(spawnedPrefabs[i].gameObject);
                            }
                        }

                        spawnedPrefabs.Clear();
                    }
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                public void instantiateParticlePrefab(Vector2 mousePosition, float maxDistance)
                {
                    if (spawnedPrefabs != null)
                    {
                        if (!disableSpawn)
                        {
                            Vector3 position = mousePosition;

                            position.z = maxDistance;
                            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(position);

                            Vector3 directionToWorldMouse = worldMousePosition - Camera.main.transform.position;

                            RaycastHit rayHit;
                            Physics.Raycast(Camera.main.transform.position, directionToWorldMouse, out rayHit, maxDistance);

                            Vector3 spawnPosition;

                            if (rayHit.collider)
                            {
                                spawnPosition = rayHit.point;
                            }
                            else
                            {
                                spawnPosition = worldMousePosition;
                            }

                            ParticleSystems prefab = particlePrefabs[currentParticlePrefab];

                            ParticleSystems newParticlePrefab = Instantiate(
                                prefab, spawnPosition, prefab.transform.rotation) as ParticleSystems;

                            newParticlePrefab.gameObject.SetActive(true);

                            // Parent to spawner.

                            newParticlePrefab.transform.parent = transform;

                            spawnedPrefabs.Add(newParticlePrefab);
                        }
                    }
                }

                // ...

                //public void instantiateParticlePrefabRandom()
                //{
                //    if (!disableSpawn)
                //    {
                //        instantiateParticlePrefab(new Vector3(
                //            Random.Range(0.0f, Screen.width), Random.Range(0.0f, Screen.height), 0.0f));
                //    }
                //}

                // ...

                public void randomize()
                {
                    currentParticlePrefab = Random.Range(0, particlePrefabs.Count);
                }

                // Get particle count from all spawned.

                public override int getParticleCount()
                {
                    int pcount = 0;

                    if (spawnedPrefabs != null)
                    {
                        for (int i = 0; i < spawnedPrefabs.Count; i++)
                        {
                            if (spawnedPrefabs[i])
                            {
                                pcount += spawnedPrefabs[i].getParticleCount();
                            }
                            else
                            {
                                spawnedPrefabs.RemoveAt(i);
                            }
                        }
                    }

                    return pcount;
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
