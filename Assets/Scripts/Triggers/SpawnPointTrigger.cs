using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Vector3 spawnPointOffset;

    private void Awake()
    {
        
    }

    public void Triggered()
    {
        Global.respawnPoint = transform.position + spawnPointOffset;
        gameObject.SetActive(false);
    }
}
