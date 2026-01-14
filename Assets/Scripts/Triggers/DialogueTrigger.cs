using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] int id;
    [SerializeField] string dialogueKey;

    public void Triggered()
    {
        if (Global.dialogueTriggers.Length <= id)
        {
            gameObject.SetActive(false);
            return;
        }
        if (!Global.dialogueTriggers[id])
        {
            Global.dialogueTriggers[id] = true;
            DialogueManager.Instance.WriteDialogue(dialogueKey);
            SaveManager.SaveGame();
        }
        gameObject.SetActive(false);
    }
}
