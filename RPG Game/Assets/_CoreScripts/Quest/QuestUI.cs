using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{

    [SerializeField] public Text questName;
    [SerializeField] public Text questDescription;
    [SerializeField] GameObject quest1;
    [SerializeField] GameObject checkMark;


    Quest currentQuest;

    private void Start()
    {
        questDescription.text = "";
        questName.text = "";
        
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest;
    }

    public void SetCurrentQuest(Quest quest)
    {
        currentQuest = quest;
    }

    public void PopulateQuestUI(string questName, string questDescription)
    {
        this.questName.text = questName;
        this.questDescription.text = questDescription;
    }

    public void CleanQuestUI()
    {
        this.questName.text = "";
        this.questDescription.text = "";
    }


    public void SetQuestUIVisibility(bool b)
    {
        quest1.SetActive(b);
    }

    public void SetCheckmarkVisibility(bool b)
    {
        checkMark.SetActive(b);
    }

    public void UpdateQuestDescription()
    {
        this.questDescription.text = currentQuest.questDescription;
    }

}
