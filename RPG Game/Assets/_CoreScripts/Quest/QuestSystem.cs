using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;

public class QuestSystem : MonoBehaviour
{

    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] Dialogue startDialogue;
    [SerializeField] Dialogue givingQuestDialogue;
    [SerializeField] Dialogue endQuestDialogue;
    [SerializeField] Dialogue comingBackToNPCNotFinishingQuest;
    [SerializeField] Dialogue afterFinishingQuestDialogue;

    [SerializeField] string questType;
    [SerializeField] GameObject quests;


    private EnemyAI enemyAI;
    private Quest quest;
    private bool isQuestAssigned;
    private bool hasFinishedQuest;
    private bool hasRecivedReward;
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
        dialogueManager.AddToDialogue(givingQuestDialogue);
        isQuestAssigned = true;
        quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
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
    }
}
