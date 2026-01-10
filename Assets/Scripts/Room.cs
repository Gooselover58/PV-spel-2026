using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 enterPoint;
    [SerializeField] Vector2 respawnPoint;

    public void EnterRoom()
    {
        Global.respawnPoint = respawnPoint;
        Global.playerTrans.gameObject.SetActive(true);
        UIManager.Instance.SetUIState("Death", false);
        PlayerMovement.canMove = true;
    }
}
