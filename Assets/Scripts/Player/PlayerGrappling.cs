using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D rbG;
    private Transform grapplingHookHolder;

    private Coroutine grappleRoutine;

    private Queue<GameObject> grapplingHooks = new Queue<GameObject>();

    private Vector2 maintainedVelocity;
    private GameObject grappleHook;
    private GameObject spawnHook;
    private GameObject currentSpawnHook;
    private LineRenderer rope;
    private float grappleCooldown;
    private int remainingGrapples;

    public PlayerState playerState;
    private Vector2 movementInput;

    [SerializeField] float grapplePower;
    [SerializeField] float grappleWindup;
    [SerializeField] float grappleTime;
    [SerializeField] float grappleCooldownTarget;
    [SerializeField] int baseGrapples;
    [SerializeField] float ropeWidth;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionBoost;

    private void Awake()
    {
        Global.playerGrappling = this;
        rb = GetComponent<Rigidbody2D>();
        grapplingHookHolder = GameObject.FindGameObjectWithTag("GrapplingHolder").transform;

        grappleHook = Resources.Load<GameObject>("Prefabs/Grapple hook");
        grappleRoutine = null;
        remainingGrapples = baseGrapples;

        rope = GetComponentInChildren<LineRenderer>();
        rope.enabled = false;
        rope.positionCount = 2;
        rope.widthMultiplier = ropeWidth;
        rope.material = new Material(Shader.Find("Sprites/Default"));

        CreateGrappleObjects(5);
    }

    private void OnEnable()
    {
        grappleRoutine = null;
        DisableGrappleObjects();
        ResetGrapples();
    }

    public void ResetPlayer()
    {
        Global.isPlayerHoldingBomb = false;
        playerState = PlayerState.FREE;
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;
        rb.drag = 0;
        rope.enabled = false;
    }

    private void Update()
    {
        grappleCooldown -= Time.deltaTime;

        // Gets input from player
        movementInput = InputManager.Instance.GetMovement();

        if (InputManager.Instance.GetInputDown("Grapple") && movementInput.magnitude > 0 && grappleCooldown < 0 && remainingGrapples > 0)
        {
            Grapple();
        }

        if (rope.enabled == true)
        {
            rope.SetPosition(0, transform.position);
            rope.SetPosition(1, currentSpawnHook.transform.position);
        }
    }

    private void CreateGrappleObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            spawnHook = Instantiate<GameObject>(grappleHook, transform.position, Quaternion.identity, grapplingHookHolder);
            grapplingHooks.Enqueue(spawnHook);
            spawnHook.SetActive(false);
        }
    }

    private void DisableGrappleObjects()
    {
        foreach (Transform hook in grapplingHookHolder)
        {
            GameObject hookOb = hook.gameObject;
            hookOb.SetActive(false);
            if (!grapplingHooks.Contains(hookOb))
            {
                grapplingHooks.Enqueue(hookOb);
            }
        }
    }

    public void ResetGrapples()
    {
        remainingGrapples = baseGrapples;
        /*if (grappleRoutine == null)
        {
            remainingGrapples = baseGrapples;
        }*/
    }

    public void SetGrapples(int amount)
    {
        remainingGrapples = amount;
    }

    private void Grapple()
    {
        //The player cant move while grappleing
        PlayerMovement.canMove = false;
        rb.gravityScale = 0;

        maintainedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;

        currentSpawnHook = grapplingHooks.Dequeue();

        //the grappling stops and restarts
        if (grappleRoutine != null)
        {
            StopCoroutine(grappleRoutine);
            DisableGrappleObjects();
        }
        grappleRoutine = StartCoroutine(GrappleDuration());
        grappleCooldown = grappleCooldownTarget;
    }

    private float GetGrappleWindup(Vector2 dir)
    {
        float windupTime = grappleWindup;
        // Gets the distance that the hook will travel during the windup
        float distance = (grapplePower * 2 / rb.mass) * grappleWindup;

        // Fires three rays in the grapple direction to look for ground collisions
        RaycastHit2D[] rays = new RaycastHit2D[1];
        rays[0] = Physics2D.Raycast(transform.position, dir, distance, Global.groundLayer);
        //rays[1] = Physics2D.Raycast(transform.position + new Vector3(0, 0.25f, 0), dir, distance, Global.groundLayer);
        //rays[2] = Physics2D.Raycast(transform.position + new Vector3(0, -0.25f, 0), dir, distance, Global.groundLayer);
        foreach (RaycastHit2D ray in rays)
        {
            if (ray.collider != null)
            {
                // If there is an object in the way, decrease the windup time depending on the distance from the player to the object
                windupTime *= (ray.distance / distance);
                break;
            }
        }
        return windupTime;
    }

    private IEnumerator GrappleDuration() // What happens while grappling
    {
        // Gets grappling direction with the players input
        Vector2 grappleDirection = new Vector2(movementInput.x, movementInput.y).normalized;

        // Changes windup time if there is any objects in the grapple direction
        float windup = GetGrappleWindup(grappleDirection);

        // Sets hook object to the right position and moves it unless an object is too close
        if (windup > 0.015f)
        {
            rope.enabled = true;
            currentSpawnHook.transform.position = transform.position;
            currentSpawnHook.SetActive(true);
            rbG = currentSpawnHook.GetComponent<Rigidbody2D>();
            rbG.AddForce(grappleDirection * grapplePower * 2, ForceMode2D.Impulse);
        }

        // Waits
        yield return new WaitForSeconds(windup);

        // Sets player in the grappling state
        playerState = PlayerState.GRAPPLING;

        // Decreases remaining grapples the player can do before touching the ground again
        remainingGrapples--;

        // Sets speed to 0
        rbG.velocity = Vector2.zero;

        // Moves player
        rb.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(grappleTime);

        // Sets player to be in the free state
        playerState = PlayerState.FREE;

        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;

        // Resets players maintained velocity if not in the same direction as the grapple
        Vector2 normalizedMV = maintainedVelocity.normalized;

        //Debug.Log($"{normalizedMV.x}, {grappleDirection.x}, {maintainedVelocity.x}");
        maintainedVelocity.x = (normalizedMV.x == grappleDirection.x) ? maintainedVelocity.x : 0;
        maintainedVelocity.y = (normalizedMV.y == grappleDirection.y) ? maintainedVelocity.y : 0;

        // Sets the player speed to what was before grappeling (CURRENTLY DISABLED FOR TESTING PURPOSES)
        //rb.velocity += maintainedVelocity;

        rope.enabled = false;
        grapplingHooks.Enqueue(spawnHook);
        currentSpawnHook.SetActive(false);
        grappleRoutine = null;
    }

    // If resetPlayer is false then the players variables will not be reset
    private void CancelGrapple(bool resetPlayer)
    {
        // Resets all variables affected by grappling
        rope.enabled = false;
        if (resetPlayer)
        {
            ResetPlayer();
        }

        // Ensures the grappling routine has been stopped
        if (grappleRoutine != null)
        {
            StopCoroutine(grappleRoutine);
            grappleRoutine = null;
        }

        // Disables grappling objects since the routine will be unable to do so
        DisableGrappleObjects();
    }

    private void Explode()
    {
        // Work in progress
        // Creates an explosion that deactivates all Fragile objects
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius, Global.groundLayer);
        EffectManager.Instance.PlayParticles("Explosion", transform.position, 50);
        foreach (Collider2D col in cols)
        {
            if (col.CompareTag("Fragile"))
            {
                col.gameObject.SetActive(false);
            }
        }
        Global.playerMovement.BoostEntity(Vector2.up, explosionBoost);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Only proceed if player is grappling
        if (playerState != PlayerState.GRAPPLING)
        {
            return;
        }

        bool resetPlayer = true;
        // If player has a bomb, explode
        // Otherwise attach to surface if possible
        if (Global.isPlayerHoldingBomb)
        {
            Global.isPlayerHoldingBomb = false;
            Explode();
        }
        else if (col.gameObject.CompareTag("Grappleable"))
        {
            resetPlayer = false;
            playerState = PlayerState.ATTACHED;
            rb.velocity = Vector2.zero;
        }

        // Cancel the grapple to reset variables
        CancelGrapple(resetPlayer);
    }

    public enum PlayerState
    {
        FREE, GRAPPLING, ATTACHED
    }
}
