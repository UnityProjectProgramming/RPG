using UnityEngine;
using System.IO;
using System.Text;
using System;

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
                Transform playerTransform = GetPlayerTransform();
                byte[] buffer = SerializeVector(playerTransform.position);
                stream.Write(buffer, 0, buffer.Length);
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
                Vector3 playerPos = DeseralizeVector(buffer);
                SetPlayerTransform(playerPos);
            }
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * sizeof(float)];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }

        private Vector3 DeseralizeVector(byte[] buffer)
        {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, 4);
            result.z = BitConverter.ToSingle(buffer, 8);
            return result;
        }

        private Transform GetPlayerTransform()
        {
            return GameObject.FindWithTag("Player").transform;
        }

        private void SetPlayerTransform(Vector3 newPos)
        {
            GameObject.FindWithTag("Player").transform.position = newPos;
        }

        private string GetPathFromSavingFile(string saveFile)
        {
            saveFile += ".sav";
            return Path.Combine(Application.persistentDataPath, saveFile);
        }
    }
}

