using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Saving;

public class HeavySoldireKiller : Quest
{
    // TODO Implement isaveable interface

    void Start ()
    {
        AssignQuest();

    }

    void AssignQuest()
    {
        questName = "Heavy Soldire Killer";
        questDescription = "<color=#6c6250ff>" + 0 + "</color>/ " + 2 + " Heavy Soldire Killed";
        experienceAmountReward = 100;

        // Reward item....

        Debug.Log("Starting quest: " + questName);
        QuestUI questUI = (QuestUI)FindObjectOfType(typeof(QuestUI));
        questUI.PopulateQuestUI(questName, questDescription);
        
        goals.Clear();
        goals.Add(new KillGoal(this, EnemyType.HeavySoldire, " Kill 2 Heavy Soldires ", false, 0, 1));
        goals.ForEach(g => g.Init());
    }

}
