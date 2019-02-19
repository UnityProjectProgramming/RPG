using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestUI : MonoBehaviour {

    [SerializeField] GameObject questCanvas;
    [SerializeField] Text questName;
    [SerializeField] Text questDescription;


    public void ActivateQuestUI()
    {
        questCanvas.SetActive(true);
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

    public void DisableQuestUI()
    {
        questCanvas.SetActive(false);
    }
}
