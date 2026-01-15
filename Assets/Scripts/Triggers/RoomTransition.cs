using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour, ITrigger
{
    private Room room;
    [SerializeField] Room destination;

    private void Awake()
    {
        room = transform.parent.parent.GetComponent<Room>();
    }

    public void Triggered()
    {
        if (destination == null)
        {
            Debug.LogError("Destination not found");
            return;
        }
        Global.playerMovement.FreezeMovement();
        UIManager.Instance.SetUIState("Death", true);
        room.gameObject.SetActive(false);
        Global.playerTrans.gameObject.SetActive(false);
        GameManager.Instance.ChangeRoom(destination);
    }
}
