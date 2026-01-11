using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour, ITrigger
{
    private BoxCollider2D col;

    private Coroutine cooldownRoutine;

    public float resetTime;

    protected virtual void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    protected void OnEnable()
    {
        ResetPowerup();
    }

    public virtual void Triggered()
    {

    }

    protected void CoolDown()
    {
        col.enabled = false;
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }
        StartCoroutine(CoolDownTime());
    }

    protected IEnumerator CoolDownTime()
    {
        yield return new WaitForSeconds(resetTime);
        ResetPowerup();
    }

    protected virtual void ResetPowerup()
    {
        cooldownRoutine = null;
        col.enabled = true;
    }
}
