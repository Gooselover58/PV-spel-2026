using System;
using System.Collections;
using System.Collections.Generic;
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

    private TextAsset dialogueFile;
    private Dictionary<string, Dialogue> dialogueHolder = new Dictionary<string, Dialogue>();

    private Coroutine writingRoutine;

    [SerializeField] float letterInterval;

    private void Awake()
    {
        LoadDialogue();
    }

    private void LoadDialogue()
    {
        dialogueFile = Resources.Load<TextAsset>("Text/Dialogue");
        Scene[] scenes = JsonUtility.FromJson<Scene[]>(dialogueFile.text);
        foreach (Scene scene in scenes)
        {
            foreach (Dialogue dialogue in scene.dialogue)
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
        string text = dialogue.text;
        string writtenText = "";
        for (int i = 0; i < text.Length; i++)
        {
            writtenText += text[i];
            UIManager.Instance.ChangeDialogueText(writtenText);
            yield return new WaitForSeconds(letterInterval);
        }
        yield return new WaitForSeconds(3);
        if (dialogue.next != null)
        {
            WriteDialogue(dialogue.next);
        }
    }
}

[Serializable]
public class Scene
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