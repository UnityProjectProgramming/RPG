using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSlayer : Quest
{
    // TODO Implement isaveable interface

    void Start ()
    {
        AssignQuest();

    }

    void AssignQuest()
    {
        questName = "Boss Slayer";
        questDescription = "<color=#6c6250ff>" + 0 + "</color>/ " + 1 + " Gate Keeper Killed";
        experienceAmountReward = 100;

        // Reward item....

        Debug.Log("Starting quest: " + questName);
        QuestUI questUI = (QuestUI)FindObjectOfType(typeof(QuestUI));
        questUI.PopulateQuestUI(questName, questDescription);
        
        goals.Clear();
        goals.Add(new KillGoal(this, EnemyType.Grunt, "Kill the gate Keeper", false, 0, 1));
        goals.ForEach(g => g.Init());
    }
}
