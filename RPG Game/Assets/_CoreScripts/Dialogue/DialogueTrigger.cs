using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    QuestSystem questSystem;

    private void Start()
    {
        questSystem = GetComponent<QuestSystem>();
    }

    public void TriggerDialogue()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        questSystem.Interact();
    }

}
