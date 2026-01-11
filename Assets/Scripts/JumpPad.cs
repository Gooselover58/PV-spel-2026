using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float boostPower;
    [SerializeField] bool useBoostModifier;

    private float coolDown;

    private void Awake()
    {
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
            entity.BoostEntity(Vector2.up, power);
            coolDown = 0.1f;
        }
    }
}
