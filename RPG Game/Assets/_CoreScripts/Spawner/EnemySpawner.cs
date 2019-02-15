using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class EnemySpawner : MonoBehaviour
{
    EnemyType enemyTypeToSpawn;
    Transform spawnPos;
    //12.40
    private void FixedUpdate()
    {
        ObjectPooler.Instance.SpawnFromPool(enemyTypeToSpawn, spawnPos.position, spawnPos.rotation);
    }
}
