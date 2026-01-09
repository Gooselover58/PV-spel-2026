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

    private void Awake()
    {
        Global.groundLayer = LayerMask.GetMask("Ground");
        Global.hazardLayer = LayerMask.GetMask("Hazard");
    }

    private void Start()
    {
        playerTrans = Global.playerTrans;
        DialogueManager.Instance.WriteDialogue("Intro_01");
    }

    public void RespawnPlayer()
    {
        playerTrans.gameObject.SetActive(false);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);

        playerTrans.position = Global.respawnPoint;
        playerTrans.gameObject.SetActive(true);
    }
}
