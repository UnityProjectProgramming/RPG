using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Saving;

public class HeavySoldireSlayer : Quest
{
    // TODO Implement isaveable interface
    // Use this for initialization
    void Start ()
    {
        AssignQuest();

    }

    void AssignQuest()
    {
        questName = "Heavy Soldire Slayer";
        questDescription = "<color=#6c6250ff>" + 0 + "</color>/ " + 1 + " Heavy Soldire Killed";
        experienceAmountReward = 100;

        // Reward item....

        Debug.Log("Starting quest: " + questName);
        QuestUI questUI = (QuestUI)FindObjectOfType(typeof(QuestUI));
        questUI.PopulateQuestUI(questName, questDescription);
        
        goals.Clear();
        goals.Add(new KillGoal(this, EnemyType.HeavySoldire, "Kill 1 soldire", false, 0, 1));
        goals.ForEach(g => g.Init());
    }

}
