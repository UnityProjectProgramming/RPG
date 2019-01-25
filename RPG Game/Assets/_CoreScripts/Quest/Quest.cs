using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour
{
    public List<Goal> goals = new List<Goal>();
    public string QuestName;
    public string Questdescription;
    public int experienceAmountReward;
    // We can add here and item as a reward such as sword, potion, key .... etc.
    public bool completed;

    public void CheckGoals()
    {
        completed = goals.All(g => g.completed);
        if(completed)
        {
            GiveReward();
        }
    }

    private void GiveReward()
    {
        // Handle Giving rewards here and increasing Player EXP.
    }

}
