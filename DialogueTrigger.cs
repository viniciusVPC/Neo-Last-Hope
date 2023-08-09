using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueStage[] dialogues;
    DialogueManager dialogueManager;
    [SerializeField] public int currentStage = 1;
    public bool talkedOnce;

    [SerializeField] GameObject[] ExternalTriggers;
    

    [TextArea(3, 1)]
    public string text;

    public Sprite[] emotionFaces;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        if(ExternalTriggers != null)
        {
            changeTrigger();
        }
    }

    public void changeTrigger()
    {
        int triggersQuantity = 0;
        foreach (GameObject triggerObject in ExternalTriggers)
            {
                if (triggerObject != null && triggerObject.GetComponent<PublicTriggerController>().publicTrigger)
                {
                    triggersQuantity++;
                }
            }
            currentStage = 1 + triggersQuantity;
    }

    public void TriggerDialogue()
    {
        if (currentStage == 1 && talkedOnce)
        {
            dialogueManager.StartDialogue(dialogues[0].getDialogue(), this.gameObject);
        }
        else dialogueManager.StartDialogue(dialogues[currentStage].getDialogue(), this.gameObject);
        if(this.CompareTag("PC"))
        {
            this.GetComponentInParent<SpriteRenderer>().sprite = emotionFaces[dialogueManager.currentEmotion];
        }
    }

    public void NextDialogue()
    {
        dialogueManager.DisplayNextSentence();
        if (this.CompareTag("PC"))
        {
            this.GetComponentInParent<SpriteRenderer>().sprite = emotionFaces[dialogueManager.currentEmotion];
        }
    }
}
