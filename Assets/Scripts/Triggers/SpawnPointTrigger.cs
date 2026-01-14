using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Vector3 spawnPointOffset;

    public void Triggered()
    {
        Global.respawnPoint = transform.position + spawnPointOffset;
        gameObject.SetActive(false);
    }
}
