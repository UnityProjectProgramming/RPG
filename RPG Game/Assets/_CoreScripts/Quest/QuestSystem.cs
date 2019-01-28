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

    // Use this for initialization
    void Start ()
    {
        isQuestAssigned = false;
        hasFinishedQuest = false;
        enemyAI = GetComponent<EnemyAI>();
	}
	
    public void Interact()
    {
        dialogueManager.StartDialogue(startDialogue);
        if(!isQuestAssigned && !hasFinishedQuest)
        {
            AssignQuest();
        }
        else if(isQuestAssigned && !hasFinishedQuest)
        {
            CheckQuest();
        }
        else
        {
            // Add Dialogue: Thanks for that stuff that one time
            //dialogueManager.StartDialogue(afterFinishingQuestDialogue);
            dialogueManager.AddToDialogue(afterFinishingQuestDialogue);
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
            quest.GiveReward();
            hasFinishedQuest = true;
            isQuestAssigned = false;
            // Show smthing on Dialogue for ex: Thanks for that here is ur reward....etc          
            dialogueManager.AddToDialogue(endQuestDialogue);
        }
        else
        {
            // add dialogue: you need to finish the quest !
            dialogueManager.AddToDialogue(comingBackToNPCNotFinishingQuest);
        }
    }
}
