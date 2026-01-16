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

    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();

    private GameObject dialogueObject;
    private TextMeshProUGUI dialogueText;
    private GameObject deathScreenObject;
    private GameObject settingsMenu;

    public bool canOpenSettings;
    public bool hasSettings;

    private void Awake()
    {
        LoadUI();
        SetUIState("Dialogue", false);
        SetUIState("Death", false);
        SetUIState("Settings", false);
    }

    private void LoadUI()
    {
        canOpenSettings = true;
        hasSettings = false;

        Transform parentCanvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        canvas = parentCanvas.GetChild(0);
        dialogueObject = canvas.GetChild(0).gameObject;
        dialogueText = dialogueObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        deathScreenObject = canvas.GetChild(1).gameObject;
        settingsMenu = parentCanvas.GetChild(2).gameObject;

        canvas.gameObject.SetActive(true);
        dialogueText.text = "";

        uiElements.Add("Dialogue", dialogueObject);
        uiElements.Add("Death", deathScreenObject);
        uiElements.Add("Settings", settingsMenu);
    }

    private void Update()
    {
        if (InputManager.Instance.GetInputDown("Settings") && canOpenSettings)
        {
            bool newSettingsState = !hasSettings;
            SetUIState("Settings", newSettingsState);
            hasSettings = newSettingsState;
        }
    }

    public void DisableSettings()
    {
        hasSettings = false;
        SetUIState("Settings", false);
    }

    public void SetUIState(string elementKey, bool state)
    {
        if (uiElements.ContainsKey(elementKey))
        {
            GameObject element = uiElements[elementKey];
            element.SetActive(state);
        }
        else
        {
            Debug.LogError($"Could not find {elementKey} in UI dictionary");
        }
    }

    public void ChangeDialogueText(string text)
    {
        if (dialogueObject.activeSelf)
        {
            dialogueText.text = text;
        }
    }
}
