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

    private List<Room> levels = new List<Room>();

    private void Awake()
    {
        Global.groundLayer = LayerMask.GetMask("Ground");
        Global.deaths = 0;

        Transform levelHolder = GameObject.FindGameObjectWithTag("Levels").transform;
        foreach (Transform level in levelHolder)
        {
            Room room = level.GetComponent<Room>();
            if (room != null)
            {
                levels.Add(room);
            }
        }

        SetRoom(levels[0]);
    }

    private void Start()
    {
        playerTrans = Global.playerTrans;
        //DialogueManager.Instance.WriteDialogue("Intro_01");
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
        Global.currentRoom.ResetRoom();

        yield return new WaitForSeconds(1f);

        if (Global.deaths == 5)
        {
            DialogueManager.Instance.WriteDialogue("Deaths_5");
        }
        else if (Global.deaths == 10)
        {
            DialogueManager.Instance.WriteDialogue("Deaths_10");
        }

        // Removes the respawn screen, respawns the player and resets their variables
        playerTrans.gameObject.SetActive(true);
        UIManager.Instance.SetUIState("Death", false);
        Global.playerGrappling.ResetPlayer();
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

        SetRoom(destination);
    }

    private void SetRoom(Room room)
    {
        Global.currentRoom = room;
        room.gameObject.SetActive(true);
        room.EnterRoom();
    }
}
