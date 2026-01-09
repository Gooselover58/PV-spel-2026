using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private Transform canvas;
    private GameObject dialogueObject;
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        LoadUI();
        SetDialogueState(false);
    }

    private void LoadUI()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        dialogueObject = canvas.GetChild(0).gameObject;
        dialogueText = dialogueObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        dialogueText.text = "";
    }

    public void SetDialogueState(bool state)
    {
        dialogueObject.SetActive(state);
    }

    public void ChangeDialogueText(string text)
    {
        if (dialogueObject.activeSelf)
        {
            dialogueText.text = text;
        }
    }
}
