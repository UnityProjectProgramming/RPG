using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour {

    private QuestManager questManager;

    public int questNumber;
    public bool startQuest;
    public bool endQuest;


	void Start ()
    {
        questManager = GameObject.FindObjectOfType<QuestManager>();
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if(!questManager.questCompleted[questNumber])
            {
                if(startQuest && !questManager.quests[questNumber].gameObject.activeSelf)
                {
                    questManager.quests[questNumber].gameObject.SetActive(true);
                    questManager.quests[questNumber].StartQuest();
                }
                if(endQuest && questManager.quests[questNumber].gameObject.activeSelf)
                {
                    questManager.quests[questNumber].EndQuest();
                }
            }
        }
    }
}
