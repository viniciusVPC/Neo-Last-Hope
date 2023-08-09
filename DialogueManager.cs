using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    [SerializeField] GameObject dialogueCanvas;

    public GameObject PCCanvas;
    [SerializeField] Text textoEscrever;
    Controle controle;
    public bool isPC;
    public int currentEmotion;

    GameObject parentNPC = null;

    Queue<int> emotions;
    Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        emotions = new Queue<int>();
        controle = GameObject.FindWithTag("Player").GetComponent<Controle>();
    }

    private void Update()
    {
        if (isPC)
        {
            if (controle.PCIsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    controle.PCIsOpen = false;
                    isPC = false;
                    parentNPC.GetComponentInParent<SpriteRenderer>().sprite = parentNPC.GetComponent<DialogueTrigger>().emotionFaces[11];

                }
            }
        }
    }

    public void StartDialogue(Dialogue[] dialogue, GameObject parentNPC)
    {
        this.parentNPC = parentNPC;
        dialogueCanvas.SetActive(true);

        sentences.Clear();

        foreach (Dialogue dialogo in dialogue)
        {
            emotions.Enqueue(dialogo.emotion);
            sentences.Enqueue(dialogo.sentence);
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

        currentEmotion = emotions.Dequeue();
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
            yield return null;
        }
    }

    void EndDialogue()
    {
        controle.isTalking = false;
        dialogueCanvas.SetActive(false);
        parentNPC.GetComponent<DialogueTrigger>().talkedOnce = true;
        Debug.Log("acabou");

        if (isPC)
        {
            PCCanvas.SetActive(true);
            controle.PCIsOpen = true;
            currentEmotion = 0;
        }
    }
}
