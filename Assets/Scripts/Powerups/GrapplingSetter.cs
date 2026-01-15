using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingSetter : Powerup
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] Sprite deactiveSprite;

    [SerializeField] Color particleColor;
    [SerializeField] int setGrappleAmount;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        anim.SetBool("IsActive", true);
        deactiveSprite = Resources.LoadAll<Sprite>("Sprites/HeartBetter")[2];
    }

    public override void Triggered()
    {
        col.enabled = false;
        anim.SetBool("IsActive", false);
        sr.sprite = deactiveSprite;
        Global.playerGrappling.SetGrapples(setGrappleAmount);
        EffectManager.Instance.PlayParticles("GrappleReset", transform.position, 15, particleColor);
        CoolDown();
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();
        anim.SetBool("IsActive", true);
    }
}
