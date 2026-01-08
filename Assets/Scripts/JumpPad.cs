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

    private void BoostObject(Rigidbody2D objectRb)
    {
        float power = (useBoostModifier) ? boostPower * Global.jumpPadBoostModifier : boostPower;
        objectRb.AddForce(Vector2.up * power, ForceMode2D.Impulse);
        coolDown = 0.1f;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D objectRb = col.gameObject.GetComponent<Rigidbody2D>();
        Entity entity = col.gameObject.GetComponent<Entity>();
        if (objectRb != null && entity != null && coolDown <= 0)
        {
            BoostObject(objectRb);
        }
    }
}
