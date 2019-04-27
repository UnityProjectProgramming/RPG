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
            FileStream stream = File.Open(path, FileMode.Create);
            byte[] bytes = Encoding.UTF8.GetBytes("Kore De Owari Da !!");
            stream.Write(bytes, 0, bytes.Length);
            stream.Close(); // main reason why we close file is because when we File.Open it sends a msg to the OS to give us a file handle and we must free the file handle incase we run out of resuource
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

