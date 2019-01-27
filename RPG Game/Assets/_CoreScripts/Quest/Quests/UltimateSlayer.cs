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
        questDescription = "Do things";
        experienceAmountReward = 100;
        // Reward item....

        goals.Add(new KillGoal(EnemyType.Minion, "Kill 4 Minions", false, 0, 4));
        goals.Add(new KillGoal(EnemyType.Knight, "Kill 2 knights", false, 0, 2));

        goals.ForEach(g => g.Init());
	}

}
