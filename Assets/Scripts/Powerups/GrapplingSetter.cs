using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingSetter : Powerup
{
    private SpriteRenderer sr;

    private Color activeColor;
    private Color deactiveColor;

    [SerializeField] int setGrappleAmount;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        activeColor = sr.color;
        deactiveColor = Color.grey;
    }

    public override void Triggered()
    {
        col.enabled = false;
        sr.color = deactiveColor;
        Global.playerGrappling.SetGrapples(setGrappleAmount);
        EffectManager.Instance.PlayParticles("GrappleReset", transform.position, 15, activeColor);
        CoolDown();
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();
        sr.color = activeColor;
    }
}
