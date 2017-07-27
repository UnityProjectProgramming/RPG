using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour {

    public string dialogue;

    public string[] dialogueLines;

    private DialogueManager dialogueManager;


	// Use this for initialization
	void Start ()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //Raycast for the mouse to the quest Guy.
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                //dialogueManager.ShowBox(dialogue);
                if(!dialogueManager.dialogueActive)
                {
                    dialogueManager.dialogueLines = this.dialogueLines;
                    dialogueManager.currentLine = 0;
                    dialogueManager.ShowDialogue();
                }
            }
        }
    }

}
