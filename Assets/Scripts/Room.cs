using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector] public Transform enterPoint;
    private Transform respawnPoint;

    private void Awake()
    {
        enterPoint = transform.GetChild(0);
        respawnPoint = transform.GetChild(1);
    }

    public void EnterRoom()
    {
        Global.respawnPoint = respawnPoint.position;
        Global.playerTrans.gameObject.SetActive(true);
        UIManager.Instance.SetUIState("Death", false);
        Global.playerGrappling.ResetPlayer();
    }
}
