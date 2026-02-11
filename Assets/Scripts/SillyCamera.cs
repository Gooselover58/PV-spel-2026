using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SillyCamera : MonoBehaviour
{
    private Volume volume;

    [SerializeField] float sillyIncrease;
    [SerializeField] float sillySpeed;
    [SerializeField] float sillyTextSpeed;
    [SerializeField] TextMeshProUGUI dialogue;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.weight = 0;
        dialogue.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && dialogue.transform.parent.gameObject.activeSelf)
        {
            StartCoroutine(DialogueThingy());
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            dialogue.transform.parent.gameObject.SetActive(false);
            volume.weight += sillyIncrease;
        }
    }

    private IEnumerator DialogueThingy()
    {
        string dialogueText = "Revolutionary gameplay!!!";
        string writtenText = "";
        for (int i = 0; i < dialogueText.Length; i++)
        {
            writtenText += dialogueText[i];
            dialogue.text = writtenText;
            yield return new WaitForSeconds(sillyTextSpeed);
        }
    }
}
