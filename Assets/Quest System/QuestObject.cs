using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour {

    public int questNumber;

    public string startDialogue;
    public string endDialogue;
    public bool isEnemyQuest;
    public string targetEnemy;
    public int enemiesToKill;
    private int enemyKillCount;

    public QuestManager questManager;

	// Use this for initialization
	void Start ()
    {
        

	}

    private void Update()
    {
        if(isEnemyQuest)
        {
            if(questManager.enemyKilled == targetEnemy)
            {
                questManager.enemyKilled = null;
                enemyKillCount++;
            }

            if(enemyKillCount>=enemiesToKill)
            {
                EndQuest();
            }
        }
    }


    public void StartQuest()
    {
        questManager.ShowQuestDialogue(startDialogue);
    }

    public void EndQuest()
    {
        questManager.ShowQuestDialogue(endDialogue);
        questManager.questCompleted[questNumber] = true;
        gameObject.SetActive(false);
    }
}
