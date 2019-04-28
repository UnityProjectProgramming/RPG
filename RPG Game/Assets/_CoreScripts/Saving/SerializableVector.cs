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

        public static Vector3 ToVector3(SerializableVector obj)
        {
            return new Vector3(obj.x, obj.y, obj.z);
        }
    }
}
