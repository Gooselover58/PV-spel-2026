using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>();
            }
            return instance;
        }
    }

    private string coolRegex = "<cool>";

    private TextAsset dialogueFile;
    private Dictionary<string, Dialogue> dialogueHolder = new Dictionary<string, Dialogue>();

    private Coroutine writingRoutine;

    [SerializeField] float letterInterval;
    [SerializeField] float textDuration;

    private void Awake()
    {
        LoadDialogue();
    }

    private void LoadDialogue()
    {
        dialogueFile = Resources.Load<TextAsset>("Text/Dialogue");
        AllDialogue allDialogue = JsonUtility.FromJson<AllDialogue>(dialogueFile.text);
        foreach (Dialogue dialogue in allDialogue.dialogue)
        {
            if (!dialogueHolder.ContainsKey(dialogue.key))
            {
                dialogueHolder.Add(dialogue.key, dialogue);
            }
        }
        writingRoutine = null;
    }

    public void WriteDialogue(string dialogueKey)
    {
        if (!dialogueHolder.ContainsKey(dialogueKey))
        {
            return;
        }

        Dialogue dialogue = dialogueHolder[dialogueKey];

        if (writingRoutine != null)
        {
            StopCoroutine(writingRoutine);
        }
        writingRoutine = StartCoroutine(DynamicWrite(dialogue));
    }

    private IEnumerator DynamicWrite(Dialogue dialogue)
    {
        string text = CheckRegex(dialogue.text);
        string writtenText = "";

        yield return new WaitForSeconds(letterInterval);
        for (int i = 0; i < text.Length; i++)
        {
            writtenText += text[i];
            UIManager.Instance.ChangeDialogueText(writtenText);
            yield return new WaitForSeconds(letterInterval);
        }
        yield return new WaitForSeconds(textDuration);
        if (dialogue.next != "")
        {
            WriteDialogue(dialogue.next);
        }
        else
        {
            UIManager.Instance.ChangeDialogueText("");
        }
    }

    private string CheckRegex(string dialogue)
    {
        string text = dialogue;
        MatchCollection matches = Regex.Matches(text, coolRegex);
        foreach (Match match in matches)
        {
            Debug.Log(match.Value);
        }
        text = Regex.Replace(text, coolRegex, "");
        return text;
    }
}

[Serializable]
public class AllDialogue
{
    public Dialogue[] dialogue;
}

[Serializable]
public class Dialogue
{
    public string key;
    public string text;
    public string next;
}