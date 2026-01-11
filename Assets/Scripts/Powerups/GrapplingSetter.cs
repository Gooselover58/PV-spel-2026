using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingSetter : Powerup
{
    private SpriteRenderer sr;

    private Color activeColor;
    private Color deactiveColor;

    [SerializeField] int grappleIncrease;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        activeColor = sr.color;
        deactiveColor = Color.grey;
    }

    public override void Triggered()
    {
        Global.playerGrappling.IncreaseGrapples(grappleIncrease);
        sr.color = deactiveColor;
        CoolDown();
    }

    protected override void ResetPowerup()
    {
        base.ResetPowerup();
        sr.color = activeColor;
    }
}
