using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    private Rigidbody2D rb;

    private Coroutine grappleRoutine;

    [SerializeField] float grapplePower;
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

        rb.AddForce(Vector2.right * grapplePower, ForceMode2D.Impulse);

        if (grappleRoutine != null)
        {
            StopCoroutine(grappleRoutine);
        }
        grappleRoutine = StartCoroutine(GrappleDuration());
    }

    private IEnumerator GrappleDuration()
    {
        yield return new WaitForSeconds(grappleTime);
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;
    }
}
