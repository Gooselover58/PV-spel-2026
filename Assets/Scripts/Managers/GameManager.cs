using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private Transform playerTrans;

    private Coroutine transitionRoutine;

    private void Awake()
    {
        Global.groundLayer = LayerMask.GetMask("Ground");
        Global.deaths = 0;
    }

    private void Start()
    {
        playerTrans = Global.playerTrans;
        DialogueManager.Instance.WriteDialogue("Intro_01");
    }

    public void RespawnPlayer()
    {
        Global.deaths += 1;
        playerTrans.gameObject.SetActive(false);
        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
        }
        transitionRoutine = StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        UIManager.Instance.SetUIState("Death", true);
        playerTrans.position = Global.respawnPoint;

        yield return new WaitForSeconds(2f);

        playerTrans.gameObject.SetActive(true);
        UIManager.Instance.SetUIState("Death", false);
    }

    public void ChangeRoom(Room destination)
    {
        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
        }
        transitionRoutine = StartCoroutine(EnterNewRoom(destination));
    }

    private IEnumerator EnterNewRoom(Room destination)
    {
        playerTrans.position = destination.enterPoint.position;

        yield return new WaitForSeconds(1f);

        destination.gameObject.SetActive(true);
        destination.EnterRoom();
    }
}
