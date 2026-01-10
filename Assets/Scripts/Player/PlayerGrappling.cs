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
    private float grappleCooldown;
    [SerializeField] int remainingGrapples;

    [SerializeField] float grapplePower;
    [SerializeField] float grappleWindup;
    [SerializeField] float grappleTime;
    [SerializeField] float grappleCooldownTarget;
    [SerializeField] int baseGrapples;

    private void Awake()
    {
        Global.playerGrappling = this;
        rb = GetComponent<Rigidbody2D>();
        grappleHook = Resources.Load<GameObject>("Prefabs/Grapple hook");
        grappleRoutine = null;
        remainingGrapples = baseGrapples;
    }

    private void Update()
    {
        grappleCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(InputManager.Instance.GetInput("Grapple")) && grappleCooldown < 0 && remainingGrapples > 0)
        {
            Grapple();
        }
    }

    public void ResetGrapples()
    {
        if (grappleRoutine == null)
        {
            remainingGrapples = baseGrapples;
        }
    }

    public void IncreaseGrapples(int amount)
    {
        remainingGrapples += amount;
    }

    private void Grapple()
    {
        //Decreases remaining grapples the player can do before touching the ground again
        remainingGrapples--;

        //The player cant move while grappeling
        PlayerMovement.canMove = false;
        rb.gravityScale = 0;

        maintainedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        //the grappling stops and restarts
        if (grappleRoutine != null)
        {
            StopCoroutine(grappleRoutine);
        }
        grappleRoutine = StartCoroutine(GrappleDuration());
        grappleCooldown = grappleCooldownTarget;
    }

    private IEnumerator GrappleDuration() // What happens while grappeling
    {
        // Spawns and moves grappling hook
        Vector2 grappleDirection = new Vector2(PlayerMovement.xInput, PlayerMovement.yInput).normalized;
        GameObject spawnHook = Instantiate<GameObject>(grappleHook, transform.position, Quaternion.identity);
        rbG = spawnHook.GetComponent<Rigidbody2D>();
        rbG.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        //Waits
        yield return new WaitForSeconds(grappleWindup);

        //Sets speed to 0
        rbG.velocity = Vector2.zero;

        //Moves player
        rb.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(grappleTime);
        
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;

        //Sets the player speed to what was before grappeling
        rb.velocity = maintainedVelocity;
        Destroy(spawnHook);
        grappleRoutine = null;
    }
}
