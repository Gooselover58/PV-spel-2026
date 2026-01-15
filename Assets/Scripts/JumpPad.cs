using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private Animator anim;

    [SerializeField] Vector2 boostDir;
    [SerializeField] float boostPower;
    [SerializeField] bool useBoostModifier;

    private float coolDown;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        coolDown = 0f;
    }

    private void Update()
    {
        coolDown -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Entity entity = col.gameObject.GetComponent<Entity>();
        if (entity != null && coolDown <= 0)
        {
            float power = (useBoostModifier) ? boostPower * Global.jumpPadBoostModifier : boostPower;
            entity.BoostEntity(boostDir, power);
            anim.SetTrigger("Bounce");
            coolDown = 0.1f;
        }
    }
}
