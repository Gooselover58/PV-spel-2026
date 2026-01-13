using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] bool zoomIn;

    public void Triggered()
    {
        Global.gameCamScript.SetFullView(zoomIn);
        UIManager.Instance.SetUIState("Dialogue", !zoomIn);
        DialogueManager.Instance.WriteDialogue("TestCool");
        gameObject.SetActive(false);
    }
}
