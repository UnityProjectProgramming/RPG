using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RPG.Characters;

public class Quest : MonoBehaviour
{
    public List<Goal> goals = new List<Goal>();
    public string questName;
    public string questDescription;
    public int experienceAmountReward;
    // We can add here and item as a reward such as sword, potion, key .... etc.
    public bool completed;

    public void CheckGoals()
    {
        completed = goals.All(g => g.completed);
    }

    public void GiveReward()
    {
        // Handle Giving rewards here and increasing Player EXP.
        Debug.Log("Giving Player Reward!");
    }

}
