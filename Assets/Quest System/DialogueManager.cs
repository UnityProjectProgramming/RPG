using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;
    public Text dialogueText;
    public string[] dialogueLines;
    public int currentLine;
    public bool dialogueActive;

	// Update is called once per frame
	void Update ()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {      
            currentLine++;
        }
        if(currentLine>=dialogueLines.Length)
        {
            dialogueBox.SetActive(false);
            dialogueActive = false;
            currentLine = 0;
        }
        dialogueText.text = dialogueLines[currentLine];
    }

    public void ShowBox(string dialogue)
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = dialogue;
    }

    public void ShowDialogue()
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);

    }
}
