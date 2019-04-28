using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableVector
    {
        public float x, y, z;


        public SerializableVector()
        {
        }

        public SerializableVector(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public  Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
