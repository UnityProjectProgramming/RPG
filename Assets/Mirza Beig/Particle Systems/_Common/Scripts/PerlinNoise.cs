
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        // =================================	
        // Classes.
        // =================================

        [System.Serializable]
        public class PerlinNoise
        {
            public void init()
            {
                offset.x = Random.Range(0.0f, 99999.0f);
                offset.y = Random.Range(0.0f, 99999.0f);
            }
            //public PerlinNoise()
            //{
            //    offset.x = Random.Range(0.0f, 99999.0f);
            //    offset.y = Random.Range(0.0f, 99999.0f);
            //}

            Vector2 offset;

            public float amplitude = 1.0f;
            public float frequency = 1.0f;

            public float value
            {
                get
                {
                    return getValue(Time.time);
                }
            }
            public float getValue(float time)
            {
                float noiseTime = time * frequency;
                return (Mathf.PerlinNoise(noiseTime + offset.x, noiseTime + offset.y) - 0.5f) * amplitude;
            }
        }

        // ...

        [System.Serializable]
        public class PerlinNoiseXYZ
        {
            public void init()
            {
                x.init();
                y.init();
                z.init();
            }

            public Vector3 xyz
            {
                get
                {
                    float time = Time.time * frequencyScale;
                    return new Vector3(x.getValue(time), y.getValue(time), z.getValue(time)) * amplitudeScale;
                }
            }

            public PerlinNoise x;
            public PerlinNoise y;
            public PerlinNoise z;

            public float amplitudeScale = 1.0f;
            public float frequencyScale = 1.0f;
        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
