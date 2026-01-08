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
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        LoadUI();
    }

    private void LoadUI()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        dialogueText = canvas.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        dialogueText.text = "";
    }

    public void ChangeDialogueText(string text)
    {
        dialogueText.text = text;
    }
}
