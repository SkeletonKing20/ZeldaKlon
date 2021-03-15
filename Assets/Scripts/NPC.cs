using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractables
{
    public string[] dialogueLines;
    int currentDialogue;
    public void Interact()
    {
        if (dialogueLines == null)
        {
            Debug.Log("Hello");
        }
        else if (currentDialogue < 0)
        {
            Debug.Log("Hello");
        }
        if (currentDialogue >= dialogueLines.Length)
        {
            currentDialogue = 0;
        }
        Debug.Log(dialogueLines[currentDialogue]);
        currentDialogue++;
    }
}
