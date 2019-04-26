using UnityEngine;
using System.IO;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Debug.Log("Saving To " + GetPathFromSavingFile(saveFile));
        }

        public void Load(string saveFile)
        {
            Debug.Log("Loading From " + GetPathFromSavingFile(saveFile));
        }

        private string GetPathFromSavingFile(string saveFile)
        {

            saveFile += ".sav";
            return Path.Combine(Application.persistentDataPath, saveFile);

        }
    }
}

