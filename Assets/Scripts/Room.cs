using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Transform essentials;

    [HideInInspector] public Transform enterPoint;
    private Transform respawnPoint;
    public List<Powerup> powerups;
    public List<GameObject> fragileObjects;

    private void Awake()
    {
        essentials = transform.GetChild(0);

        enterPoint = essentials.GetChild(0);
        respawnPoint = essentials.GetChild(1);

        Transform powerupHolder = essentials.GetChild(2);
        Transform fragilesHolder = essentials.GetChild(3);
        foreach (Transform powerup in powerupHolder)
        {
            Powerup script = powerup.GetComponent<Powerup>();
            if (script != null)
            {
                powerups.Add(script);
            }
        }
        foreach (Transform fragile in fragilesHolder)
        {
            fragileObjects.Add(fragile.gameObject);
        }
    }

    public void EnterRoom()
    {
        Global.respawnPoint = respawnPoint.position;
        Global.playerTrans.gameObject.SetActive(true);
        UIManager.Instance.SetUIState("Death", false);
        Global.playerGrappling.ResetPlayer();
    }

    public void ResetRoom()
    {
        foreach (Powerup powerup in powerups)
        {
            powerup.ResetPowerup();
        }
        foreach (GameObject fragile in fragileObjects)
        {
            fragile.SetActive(true);
        }
    }
}
