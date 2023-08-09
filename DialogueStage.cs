using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueStage
{
    [SerializeField]
    Dialogue[] dialogue;
    

    public Dialogue[] getDialogue()
    {
        return dialogue;
    }
}
