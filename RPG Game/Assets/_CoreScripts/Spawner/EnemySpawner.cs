using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] EnemyType enemyTypeToSpawn;
    [SerializeField] Transform[] spawnPositions;

    [Header("Gizmos")]
    [SerializeField] bool enableGizmos;


    Vector3 charPos = new Vector3(-106.72f, 23.39f, -185.58f);

    ObjectPooler objectPooler;

    private void Start()
    {

        objectPooler = ObjectPooler.Instance;
    }


    public void SpawnEnemies()
    {
        //spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position
        
        
        var charToSpawn = objectPooler.SpawnFromPool(enemyTypeToSpawn,
                           charPos,
                           spawnPositions[0].rotation);

        
    }

    private void OnDrawGizmos()
    {
        if(enableGizmos)
        {
            Gizmos.color = Color.green;
            foreach (Transform spawnPos in spawnPositions)
            {
                Gizmos.DrawWireSphere(spawnPos.position, 0.5f);
            }
        }
    }
}
