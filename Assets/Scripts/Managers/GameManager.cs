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

    public List<Powerup> activePowerups = new List<Powerup>();

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
        // Increases global death count
        Global.deaths += 1;

        // Disables player
        playerTrans.gameObject.SetActive(false);

        // Cancels any running transition routines, then begins respawning
        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
        }
        transitionRoutine = StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        // Displays respawn screen and sets the player back to the respawn point
        UIManager.Instance.SetUIState("Death", true);
        playerTrans.position = Global.respawnPoint;
        ResetPowerups();

        yield return new WaitForSeconds(2f);

        // Removes the respawn screen, respawns the player and resets their variables
        playerTrans.gameObject.SetActive(true);
        UIManager.Instance.SetUIState("Death", false);
        Global.playerGrappling.ResetPlayer();
    }

    private void ResetPowerups()
    {
        foreach (Powerup powerup in activePowerups)
        {
            if (!powerup.gameObject.activeSelf)
            {
                continue;
            }
            powerup.ResetPowerup();
        }
    }

    public void ChangeRoom(Room destination)
    {
        // Cancels any running transition routines, then begins room transitioning
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
