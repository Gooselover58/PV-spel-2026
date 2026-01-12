using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour, ITrigger
{
    protected BoxCollider2D col;

    private Coroutine cooldownRoutine;

    public float resetTime;

    protected virtual void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    protected void OnEnable()
    {
        ResetPowerup();
        GameManager.Instance.activePowerups.Add(this);
    }

    public virtual void Triggered()
    {

    }

    protected void CoolDown()
    {
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }
        cooldownRoutine = StartCoroutine(CoolDownTime());
    }

    protected IEnumerator CoolDownTime()
    {
        yield return new WaitForSeconds(resetTime);
        ResetPowerup();
    }

    public virtual void ResetPowerup()
    {
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }
        cooldownRoutine = null;
        col.enabled = true;
    }
}
