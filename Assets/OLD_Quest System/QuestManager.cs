using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

    public QuestObject[] quests = null;
    public DialogueManager dialogueManager;
    public bool[] questCompleted;

    public string enemyKilled;

	// Use this for initialization
	void Start ()
    {
        questCompleted = new bool[quests.Length]; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowQuestDialogue(string questDialog)
    {
        dialogueManager.dialogueLines = new string[1];
        dialogueManager.dialogueLines[0] = questDialog;
        dialogueManager.currentLine = 0;
        dialogueManager.ShowDialogue();
    }
}
