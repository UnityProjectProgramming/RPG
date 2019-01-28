using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public Quest quest;
    public string description;
    public bool completed;
    public int currentAmount;
    public int requiredAmount;

    public virtual void Init()
    {
        // Default init stuff.
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
