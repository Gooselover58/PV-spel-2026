using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] bool turnOn;

    public void Triggered()
    {
        Global.gameCamScript.SetUpsideDownCamera(turnOn);
        gameObject.SetActive(false);
    }
}
