using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class KillGoal : Goal
{
    public EnemyType enemyType;

    public KillGoal(Quest quest, EnemyType enemyType, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.quest = quest;
        this.enemyType = enemyType;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }


    public override void Init()
    {
        base.Init();
        Debug.Log("Kill Goal Init");
        CombatEvents.onEnemyDeath += EnemyDied;
    }

    void EnemyDied(Character enemy)
    {
        if (this.enemyType == enemy.GetEnemyType())
        {
            this.currentAmount++;
            Debug.Log("Current Amount now : " + currentAmount);
            UpdateQuestDescription("<color=#6c6250ff>" + currentAmount + "</color>/ " + requiredAmount + "Thugs Killed");
            Evaluate();
        }
    }

}
