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
        EnemyAI.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(EnemyAI enemy)
    {
        if(enemy.GetEnemyType() == this.enemyType)
        {
            this.currentAmount++;
            Evaluate();
        }
    }
}
