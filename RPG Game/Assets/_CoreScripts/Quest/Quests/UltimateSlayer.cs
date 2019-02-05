using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class UltimateSlayer : Quest
{


    // Use this for initialization
    void Start ()
    {

        questName = "Ultimate Slayer";
        questDescription = "Kill Thugs";
        experienceAmountReward = 100;
        // Reward item....

        Debug.Log("Starting quest: " + questName);

        goals.Add(new KillGoal(this, EnemyType.Thug, "Kill 4 Thugs", false, 0, 4));
        goals.ForEach(g => g.Init());
	}


}
