using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;

public class QuestSystem : MonoBehaviour
{
    [Header("Dialogue Manager Reference")]
    [SerializeField] DialogueManager dialogueManager;

    [Header("Dialogue")]
    [SerializeField] Dialogue startDialogue;
    [SerializeField] Dialogue givingQuestDialogue;
    [SerializeField] Dialogue endQuestDialogue;


    [Header("Questing")]
    [SerializeField] string questTypeName;
    [SerializeField] GameObject currentQuests;
    [SerializeField] QuestUI questUI;
    [SerializeField] GameObject questExclemenation;

    private EnemyAI enemyAI;
    private Quest quest;
    private bool isQuestAssigned;
    private bool hasFinishedQuest;
    private bool hasRecivedReward;

    public GameObject GetQuestsObject()
    {
        return currentQuests;
    }

    // Use this for initialization
    void Start ()
    {
        isQuestAssigned = false;
        hasFinishedQuest = false;
        hasRecivedReward = false;
        enemyAI = GetComponent<EnemyAI>();
	}
	
    public void Interact()
    {
        
        if(!isQuestAssigned && !hasFinishedQuest)
        {
            dialogueManager.StartDialogue(startDialogue);
            AssignQuest();
        }
        else if(isQuestAssigned && !hasFinishedQuest)
        {
            CheckQuest();
        }
        else
        {
        }
    }

	void AssignQuest()
    {
        questExclemenation.SetActive(false);
        dialogueManager.AddToDialogue(givingQuestDialogue);
        isQuestAssigned = true;
        quest = (Quest)currentQuests.AddComponent(System.Type.GetType(questTypeName));
    }


    void CheckQuest()
    {
        if(quest.completed)
        {
            EndQuest();
        }
        else
        {
            // add Dialogue : you need to finish the quest !
            //Debug.Log("Adding comingBackToNPCNotFinishingQuest");
        }
    }

    void EndQuest()
    {
        quest.GiveReward();
        hasFinishedQuest = true;
        isQuestAssigned = false;
        hasRecivedReward = true;
        dialogueManager.StartDialogue(endQuestDialogue);
        questUI.CleanQuestUI();
    }
}
