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

    private Dictionary<string, Sprite> anchorExpressions = new Dictionary<string, Sprite>();
    #pragma warning disable
    private string pausePattern = @"<Pause=\d";

    private TextAsset dialogueFile;
    private Dictionary<string, Dialogue> dialogueHolder = new Dictionary<string, Dialogue>();

    private SpriteRenderer anchorSprite;

    private Coroutine writingRoutine;

    [SerializeField] float letterInterval;
    [SerializeField] float textDuration;

    private void Awake()
    {
        LoadDialogue();
        LoadExpressions();
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

    private void LoadExpressions()
    {
        anchorSprite = Camera.main.transform.GetChild(1).GetComponent<SpriteRenderer>();

        Sprite[] anchorSprites = Resources.LoadAll<Sprite>("Sprites/NewsAnchor");
        foreach (Sprite sprite in anchorSprites)
        {
            string pattern = $"<{sprite.name}>";
            anchorExpressions.Add(pattern, sprite);
        }

        anchorSprite.sprite = anchorExpressions["<Normal>"];
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
        DialogueInfo info = CheckRegex(dialogue.text);
        string text = info.text;
        string writtenText = "";

        yield return new WaitForSeconds(letterInterval);
        for (int i = 0; i < text.Length; i++)
        {
            if (info.spriteChanges.ContainsKey(i))
            {
                string pattern = info.spriteChanges[i];
                anchorSprite.sprite = anchorExpressions[pattern];
                i += pattern.Length - 1;
                continue;
            }
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

    private DialogueInfo CheckRegex(string dialogue)
    {
        DialogueInfo info = new DialogueInfo();
        info.text = dialogue;
        info.spriteChanges = new Dictionary<int, string>();

        Dictionary<string, Sprite>.KeyCollection keys = anchorExpressions.Keys;
        foreach (string pattern in keys)
        {
            MatchCollection matches = Regex.Matches(dialogue, pattern);
            /*foreach (Match match in matches)
            {
                info.text = Regex.Replace(info.text, pattern, "");
            }*/
            foreach (Match match in matches)
            {
                info.spriteChanges.Add(match.Index, pattern);
            }
        }
        return info;
    }

    public class DialogueInfo
    {
        public string text;
        public Dictionary<int, string> spriteChanges;
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