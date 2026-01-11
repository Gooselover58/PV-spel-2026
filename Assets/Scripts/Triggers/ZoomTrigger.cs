using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] bool zoomIn;

    public void Triggered()
    {
        Global.gameCamScript.SetFullView(zoomIn);
        gameObject.SetActive(false);
    }
}
