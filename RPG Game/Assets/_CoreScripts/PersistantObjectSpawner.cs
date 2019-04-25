using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantGameObject;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistantObjects();
            hasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            DontDestroyOnLoad(Instantiate(persistantGameObject));
        }
    }
}

