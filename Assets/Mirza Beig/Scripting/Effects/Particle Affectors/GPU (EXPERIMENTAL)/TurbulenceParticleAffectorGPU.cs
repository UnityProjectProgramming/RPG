
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
            [System.Serializable]

            public class TurbulenceParticleAffectorGPU : ParticleAffectorGPU
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // Make sure compute shader has 
                // matching kernel methods AND pragmas.

                public enum NoiseType
                {
                    Perlin,
                    Perlin2,
                    Perlin3,
                    Perlin4,
                    Perlin5,

                    Simplex,

                    OctavePerlin,
                    OctavePerlin2,
                    OctavePerlin3,

                    OctaveSimplex,
                }

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("Affector Controls")]

                public float speed = 1.0f;

                [Range(0.0f, 8.0f)]
                public float frequency = 1.0f;

                // Unlike with its CPU counterpart,
                // the noise type cannot be changed
                // once set in Start. 

                public NoiseType noiseType = NoiseType.Perlin3;

                // ...

                [Header("Octave Variant-Only Controls")]

                [Range(1, 8)]
                public int octaves = 1;

                [Range(0.0f, 4.0f)]
                public float lacunarity = 2.0f;

                [Range(0.0f, 1.0f)]
                public float persistence = 0.5f;

                float randomX;
                float randomY;
                float randomZ;

                float offsetX;
                float offsetY;
                float offsetZ;

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
                    // Choose which main kernel method to use, 
                    // which will change the noise type used in the shader.

                    kernelNames = System.Enum.GetNames(typeof(NoiseType));

                    // ...

                    base.Start();

                    // ...

                    randomX = Random.Range(-32.0f, 32.0f);
                    randomY = Random.Range(-32.0f, 32.0f);
                    randomZ = Random.Range(-32.0f, 32.0f);
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                protected override void setShaderData()
                {
                    base.setShaderData();

                    // ...

                    shader.SetFloat("frequency", frequency);

                    shader.SetInt("octaves", octaves);

                    shader.SetFloat("lacunarity", lacunarity);
                    shader.SetFloat("persistence", persistence);

                    shader.SetVector("offsets", new Vector3(offsetX, offsetY, offsetZ));
                }

                // ...

                protected override void LateUpdate()
                {
                    float time = Time.time;

                    offsetX = (time * speed) + randomX;
                    offsetY = (time * speed) + randomY;
                    offsetZ = (time * speed) + randomZ;

                    currentKernel = (int)noiseType;

                    // ...

                    base.LateUpdate();
                }

                // ...

                protected override void OnDrawGizmosSelected()
                {
                    base.OnDrawGizmosSelected();
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
