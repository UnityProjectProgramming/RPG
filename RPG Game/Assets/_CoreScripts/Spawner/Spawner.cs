using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class Spawner : MonoBehaviour
{

    [Header("Spawning")]
    [SerializeField] Character enemyPrefab;
    [SerializeField] EnemyType enemyType;
    [SerializeField] Transform[] spawningPositions;
    [SerializeField] WaypointContainer[] patrolPaths;
    [SerializeField] [Tooltip("Delay in seconds")] float delay_GroupSpawn;
    [SerializeField] [Tooltip("Delay in seconds")] float delay_EnemySpawn;
    [SerializeField] int numberToSpawn;

    [Header("Gizmos")]
    [SerializeField] bool enableGizmos;


    //
    int spawnedEnemies;


    private void Start()
    {
        StartCoroutine(RespawnEnemies(0, 0));
        spawnedEnemies = numberToSpawn;
        CombatEvents.onEnemyDeath += EnemyDied;
    }

    void EnemyDied(Character enemy)
    {
        if (enemyType == enemy.GetEnemyType())
        {
            spawnedEnemies--;
            if(spawnedEnemies <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(RespawnEnemies(delay_GroupSpawn, delay_EnemySpawn));
                spawnedEnemies = numberToSpawn;
            }
        }
    }

    IEnumerator RespawnEnemies(float delay_GroupSpawn, float delay_EnemySpawn)
    {
        yield return new WaitForSeconds(delay_GroupSpawn);

        for (int i = 0; i < numberToSpawn; i++)
        {
            Character instantiatedChar = Instantiate(enemyPrefab, spawningPositions[i].position, spawningPositions[i].rotation);
            EnemyAI instantiatedEnemy = instantiatedChar.GetEnemyAI();
            if (instantiatedEnemy)
            {
                Debug.Log("Setting patrol path for : " + instantiatedEnemy);
                instantiatedEnemy.StopAllCoroutines();
                instantiatedEnemy.SetPatrolPath(patrolPaths[i]);
            }
            yield return new WaitForSeconds(delay_EnemySpawn);
        }
    }


    private void OnDrawGizmos()
    {
        if (enableGizmos)
        {
            Gizmos.color = Color.green;
            foreach (Transform spawnPos in spawningPositions)
            {
                Gizmos.DrawWireSphere(spawnPos.position, 0.5f);
            }
        }
    }
}
