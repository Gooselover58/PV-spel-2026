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

    private string pausePattern = @"<Pause=([0-9]+\.[0-9]+)>";
    private string fontPattern = @"<[/\/]?\w>";

    private TextAsset dialogueFile;
    private Dictionary<string, Dialogue> dialogueHolder = new Dictionary<string, Dialogue>();

    private SpriteRenderer anchorSprite;

    private Coroutine writingRoutine;

    [SerializeField] float letterInterval;
    [SerializeField] float textDuration;
    [SerializeField] float idleDelayMin, idleDelayMax;

    [SerializeField] string[] idleKeys;
    private string lastIdle;

    private void Awake()
    {
        LoadDialogue();
        LoadExpressions();
    }

    private void Start()
    {
        StartCoroutine(IdleDialogue());
    }

    private IEnumerator IdleDialogue()
    {
        if (idleKeys.Length < 3)
        {
            yield return null;
        }
        while (true)
        {
            float idleWait = UnityEngine.Random.Range(idleDelayMin, idleDelayMax);
            yield return new WaitForSeconds(idleWait);
            if (writingRoutine == null)
            {
                string dialogueKey;
                do
                {
                    int rand = UnityEngine.Random.Range(0, idleKeys.Length);
                    dialogueKey = idleKeys[rand];
                }
                while (dialogueKey != lastIdle);
                lastIdle = dialogueKey;
                WriteDialogue(dialogueKey);
            }
        }
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

        //yield return new WaitForSeconds(letterInterval);
        for (int i = 0; i < text.Length; i++)
        {
            if (info.spriteChanges.ContainsKey(i))
            {
                string pattern = info.spriteChanges[i];
                anchorSprite.sprite = anchorExpressions[pattern];
                i += pattern.Length - 1;
                continue;
            }
            else if (info.pauses.ContainsKey(i))
            {
                yield return new WaitForSeconds(info.pauses[i]);
                i += 10;
                continue;
            }
            else if (info.fontSkips.ContainsKey(i))
            {
                for (int j = 0; j < info.fontSkips[i] + 1; j++)
                {
                    writtenText += text[i + j];
                }
                i += info.fontSkips[i];
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
        info.pauses = new Dictionary<int, float>();
        info.fontSkips = new Dictionary<int, int>();

        Dictionary<string, Sprite>.KeyCollection keys = anchorExpressions.Keys;
        foreach (string pattern in keys)
        {
            MatchCollection matches = Regex.Matches(dialogue, pattern);
            foreach (Match match in matches)
            {
                info.spriteChanges.Add(match.Index, pattern);
            }
        }
        MatchCollection pauseMatches = Regex.Matches(dialogue, pausePattern);
        foreach (Match match in pauseMatches)
        {
            string value = match.Value;
            value = value.Remove(0, 7);
            value = value.Remove(value.Length - 1, 1);
            float wholeNumber = float.Parse(value.Remove(1, value.Length - 1));
            float decimalNumber = float.Parse(value.Remove(0, 2));
            float time = wholeNumber + (decimalNumber / 10);

            info.pauses.Add(match.Index, time);
        }
        MatchCollection fontMatches = Regex.Matches(dialogue, fontPattern);
        foreach (Match match in fontMatches)
        {
            info.fontSkips.Add(match.Index, match.Value.Length - 1);
        }
        return info;
    }

    public class DialogueInfo
    {
        public string text;
        public Dictionary<int, string> spriteChanges;
        public Dictionary<int, float> pauses;
        public Dictionary<int, int> fontSkips;
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