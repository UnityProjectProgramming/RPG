using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableQuest
    {
        //public List<SerializableGoal> serializableGoals = new List<SerializableGoal>();
        public string questName;
        public string questDescription;
        public int experienceAmountReward;

        public SerializableQuest(Quest quest)
        {
            questName = quest.questName;
            questDescription = quest.questDescription;
            experienceAmountReward = quest.experienceAmountReward;
        }

        public Quest ToQuest()
        {
            return new Quest(questName, questDescription, experienceAmountReward);
        }
    }
}
