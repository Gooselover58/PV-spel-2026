using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour
{
    [SerializeField] Vector2 spawnPoint;

    public void Triggered()
    {
        Global.respawnPoint = spawnPoint;
        gameObject.SetActive(false);
    }
}
