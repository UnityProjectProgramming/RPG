using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Debug.Log("Saving To " + saveFile);
        }

        public void Load(string saveFile)
        {
            Debug.Log("Loading From " + saveFile);
        }
    }
}

