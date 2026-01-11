using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckForHazards(col);
        CheckForTriggers(col);
    }

    private void CheckForHazards(Collider2D col)
    {
        if (col.gameObject.CompareTag("Hazard"))
        {
            GameManager.Instance.RespawnPlayer();
        }
    }

    private void CheckForTriggers(Collider2D col)
    {
        ITrigger trigger = col.GetComponent<ITrigger>();
        if (trigger != null)
        {
            trigger.Triggered();
        }
    }
}
