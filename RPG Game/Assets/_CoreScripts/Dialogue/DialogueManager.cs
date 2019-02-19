using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<string> sentences;

    [SerializeField] Text NPCNameText;
    [SerializeField] Text dialogueText;
    [SerializeField] Text continueDialogueText;
    [SerializeField] GameObject dialogueGameObject;

	// Use this for initialization
	void Start ()
    {
        sentences = new Queue<string>();
        dialogueGameObject.SetActive(false);
	}
	
    public void StartDialogue(Dialogue dialogue)
    {
        NPCNameText.text = dialogue.NPCName;
        dialogueGameObject.SetActive(true);
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void AddToDialogue(Dialogue dialogue)
    {
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }  
    }

    public void ContinueDialogue(Dialogue dialogue)
    {
        dialogueGameObject.SetActive(true);
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(StreamLetters(sentence));
    }

    IEnumerator StreamLetters(string sentence)
    {
        dialogueText.text = "";
        foreach(var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueGameObject.SetActive(false);
    }
}

