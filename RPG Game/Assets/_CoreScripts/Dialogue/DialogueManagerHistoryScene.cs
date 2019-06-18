using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RPG.SceneManagement;

public class DialogueManagerHistoryScene : MonoBehaviour
{

    private Queue<string> sentences;

    [SerializeField] Text dialogueText;
    [SerializeField] GameObject TextObject;


    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        
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
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }
        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(StreamLetters(sentence));
    }

    IEnumerator StreamLetters(string sentence)
    {
        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator EndDialogue()
    {
        TextObject.SetActive(false);

        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

        // Fade Out
        yield return fader.FadeOut(2);

        savingWrapper.Save();

        yield return SceneManager.LoadSceneAsync(1);

        savingWrapper.Load();


        savingWrapper.Save();

        yield return new WaitForSeconds(1);
        yield return fader.FadeIn(2);


        Destroy(gameObject);


    }


}
