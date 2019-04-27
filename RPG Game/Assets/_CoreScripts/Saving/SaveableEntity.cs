using UnityEngine;

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
            print("Capturing State for : " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Restoring State for " + GetUniqueIdentifier());
        }

        private void Update()
        {
            if(!Application.isPlaying)
            {
                Debug.Log("Editing");
            }
        }
    }
}
