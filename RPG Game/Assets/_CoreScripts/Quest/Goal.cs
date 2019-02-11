using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class Goal
{
    public Quest quest;
    public string description;
    public bool completed;
    public int currentAmount;
    public int requiredAmount;


    public virtual void Init()
    {
        Debug.Log("Goal Init");
    }

    public void Evaluate()
    {
        if(currentAmount >= requiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        quest.CheckGoals();
        completed = true;
        Debug.Log("Goal Marked Completed !");
    }

}
