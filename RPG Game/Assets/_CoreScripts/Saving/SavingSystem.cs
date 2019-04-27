using UnityEngine;
using System.IO;
using System.Text;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSavingFile(saveFile);
            Debug.Log("Saving To " + path);
            using (FileStream stream = File.Open(path, FileMode.Create)) // it will close the file on its own.
            {
                byte[] bytes = Encoding.UTF8.GetBytes("Kore De Owari Da !!");
                stream.Write(bytes, 0, bytes.Length);
            }

        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSavingFile(saveFile);
            Debug.Log("Loading From " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                string saveData = Encoding.UTF8.GetString(buffer);
                Debug.Log("Save Data: " + saveData);
            }
        }

        private string GetPathFromSavingFile(string saveFile)
        {
            saveFile += ".sav";
            return Path.Combine(Application.persistentDataPath, saveFile);
        }
    }
}

