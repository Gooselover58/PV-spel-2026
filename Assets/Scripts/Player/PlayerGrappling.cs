using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbG;

    private Coroutine grappleRoutine;
    private Vector2 maintainedVelocity;
    private GameObject grappleHook;

    [SerializeField] float grapplePower;
    [SerializeField] float grappleWindup;
    [SerializeField] float grappleTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        grappleHook = Resources.Load<GameObject>("Prefabs/Grapple hook");
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

    private IEnumerator GrappleDuration() //
    {
        Vector2 grappleDirection = new Vector2(PlayerMovement.xInput, PlayerMovement.yInput).normalized;
        GameObject spawnHook = Instantiate<GameObject>(grappleHook, transform.position, Quaternion.identity);
        rbG = spawnHook.GetComponent<Rigidbody2D>();
        rbG.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(grappleWindup);
        rbG.velocity = Vector2.zero;

        rb.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(grappleTime);
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;

        rb.velocity = maintainedVelocity;
        Destroy(spawnHook);
    }
}
