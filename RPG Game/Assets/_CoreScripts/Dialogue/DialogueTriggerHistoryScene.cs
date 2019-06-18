using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerHistoryScene : MonoBehaviour
{

    [SerializeField] Dialogue dialogue;


    public void TriggerFirstLevelDialogue()
    {
        FindObjectOfType<DialogueManagerHistoryScene>().StartDialogue(dialogue);
    }
}
