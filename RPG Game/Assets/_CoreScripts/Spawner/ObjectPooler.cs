using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public EnemyType enemyType;
        public Character characterPrefab;
        public int size; // at which point are we gonna reuse objects instead of respawning new ones.
    }

    public List<Pool> pools;
    public Dictionary<EnemyType, Queue<Character>> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    // Use this for initialization
    void Start ()
    {
        poolDictionary = new Dictionary<EnemyType, Queue<Character>>();
		
        foreach(Pool pool in pools)
        {
            Queue<Character> characterPool = new Queue<Character>();

            for (int i = 0; i < pool.size; i++)
            {
                Character character = Instantiate(pool.characterPrefab);
                character.gameObject.SetActive(false);
                characterPool.Enqueue(character);
            }

            poolDictionary.Add(pool.enemyType, characterPool);
        }

	}

    public Character SpawnFromPool(EnemyType enemyType, Vector3 pos, Quaternion rot)
    {

        if(!poolDictionary.ContainsKey(enemyType))
        {
            Debug.LogError("Pool Dictionary dsnt contain this EnemyType: " + enemyType);
            return null;
        }

        Character charToSpawn = poolDictionary[enemyType].Dequeue();

        charToSpawn.gameObject.SetActive(true);
        charToSpawn.gameObject.transform.position = pos;
        charToSpawn.gameObject.transform.rotation = rot;

        poolDictionary[enemyType].Enqueue(charToSpawn);

        return charToSpawn;
    }

}
