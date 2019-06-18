using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void DeleteSave(string saveFile)
        {
            string path = GetPathFromSavingFile(saveFile);
            File.Delete(path);
        }


        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            // Get the build index;
            if(state.ContainsKey("currentSceneBuildIndex"))
            {
                int buildIndex = (int)state["currentSceneBuildIndex"];
                // load scene async
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }

            }
            // restore the state
            RestoreState(state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSavingFile(saveFile);
            Debug.Log("Saving To " + path);
            using (FileStream stream = File.Open(path, FileMode.Create)) // it will close the file on its own.
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSavingFile(saveFile);
            Debug.Log("Loading From " + path);
            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            state["currentSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string uid = saveable.GetUniqueIdentifier();
                if(state.ContainsKey(uid))
                {
                    saveable.RestoreState(state[uid]);
                }
            }
        }

        private string GetPathFromSavingFile(string saveFile)
        {
            saveFile += ".sav";
            return Path.Combine(Application.persistentDataPath, saveFile);
        }
    }
}

