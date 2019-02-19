using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class ThugSlayer : Quest
{

    // Use this for initialization
    void Start ()
    {
        
        questName = "Thug Slayer !";
        questDescription = "Kill all the 4 thugs";
        experienceAmountReward = 100;

        // Reward item....

        Debug.Log("Starting quest: " + questName);
        QuestUI questUI = (QuestUI)FindObjectOfType(typeof(QuestUI));
        questUI.PopulateQuestUI(questName, questDescription);
        goals.Add(new KillGoal(this, EnemyType.Thug, "Kill 4 Thugs", false, 0, 4));
        goals.ForEach(g => g.Init());
	}

}
