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
    [SerializeField] float textDuration;

    private void Awake()
    {
        LoadDialogue();
    }

    private void LoadDialogue()
    {
        dialogueFile = Resources.Load<TextAsset>("Text/Dialogue");
        Scenes allScenes = JsonUtility.FromJson<Scenes>(dialogueFile.text);
        foreach (Scene scene in allScenes.scenes)
        {
            foreach (Dialogue dialogue in scene.main)
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
        yield return new WaitForSeconds(textDuration);
        if (dialogue.next != null)
        {
            WriteDialogue(dialogue.next);
        }
        else
        {
            UIManager.Instance.ChangeDialogueText("");
        }
    }
}

[Serializable]
public class Scenes
{
    public Scene[] scenes;
}

[Serializable]
public class Scene
{
    public Dialogue[] main;
}

[Serializable]
public class Dialogue
{
    public string key;
    public string text;
    public string next;
}