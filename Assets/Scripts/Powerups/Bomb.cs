using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Powerup
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] Color particleColor;
    private Color activeColor;
    private Color deactiveColor;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        activeColor = Color.white;
        deactiveColor = Color.grey;

        anim.SetBool("IsActive", true);
    }

    public override void Triggered()
    {
        col.enabled = false;
        anim.SetBool("IsActive", false);
        sr.color = deactiveColor;
        Global.isPlayerHoldingBomb = true;
        EffectManager.Instance.PlayParticles("GrappleReset", transform.position, 15, particleColor);
        CoolDown();
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();
        sr.color = activeColor;
        anim.SetBool("IsActive", true);
    }
}
