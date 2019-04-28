using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteInEditMode]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            
            return new SerializableVector(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector vec = (SerializableVector)state;
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = vec.ToVector3();
            GetComponent<NavMeshAgent>().enabled = true;
            print("Restoring State for " + GetUniqueIdentifier());
        }

        private void Update()
        {
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            Debug.Log("Editing");
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("uniqueIdentifier");

            if(string.IsNullOrEmpty(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

        }
    }
}
