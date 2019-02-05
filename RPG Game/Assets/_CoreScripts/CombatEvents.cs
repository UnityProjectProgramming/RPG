using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class CombatEvents : MonoBehaviour
{
    public delegate void EnemyEventHandler(Character enemy);
    public static event EnemyEventHandler onEnemyDeath;

    public static void EnemyDied(Character enemy)
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath(enemy);
        }
            
    }
}
