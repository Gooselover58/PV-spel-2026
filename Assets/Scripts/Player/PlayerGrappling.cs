using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    private Rigidbody2D rb;

    private Coroutine grappleRoutine;
    private Vector2 maintainedVelocity;

    [SerializeField] float grapplePower;
    [SerializeField] float grappleWindup;
    [SerializeField] float grappleTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        grappleRoutine = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.GetInput("Grapple")))
        {
            Grapple();
        }
    }

    private void Grapple()
    {
        PlayerMovement.canMove = false;
        rb.gravityScale = 0;

        maintainedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        if (grappleRoutine != null)
        {
            StopCoroutine(grappleRoutine);
        }
        grappleRoutine = StartCoroutine(GrappleDuration());
    }

    private IEnumerator GrappleDuration()
    {
        yield return new WaitForSeconds(grappleWindup);

        rb.AddForce(Vector2.right * grapplePower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(grappleTime);
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;

        rb.velocity = maintainedVelocity;
    }
}
