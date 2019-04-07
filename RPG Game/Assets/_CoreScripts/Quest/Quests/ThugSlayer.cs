using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class ThugSlayer : Quest
{

    // Use this for initialization
    void Start ()
    {
        
        questName = "Thug Slayer";
        questDescription = "<color=#6c6250ff>" + 0 + "</color>/ " + 2 + "Thugs Killed";
        experienceAmountReward = 100;

        // Reward item....

        Debug.Log("Starting quest: " + questName);
        QuestUI questUI = (QuestUI)FindObjectOfType(typeof(QuestUI));
        questUI.PopulateQuestUI(questName, questDescription);
        goals.Clear();
        goals.Add(new KillGoal(this, EnemyType.Thug, "Kill 4 Thugs", false, 0, 2));
        goals.ForEach(g => g.Init());
	}

}
