using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] int id;
    [SerializeField] string dialogueKey;

    public void Triggered()
    {
        DialogueManager.Instance.WriteDialogue(dialogueKey);
        if (Global.dialogueTriggers.Length > id)
        {
            Global.dialogueTriggers[id] = true;
        }
        gameObject.SetActive(false);
    }
}
