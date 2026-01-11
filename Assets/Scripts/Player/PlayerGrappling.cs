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
    private float grappleCooldown;
    private int remainingGrapples;

    public PlayerState playerState;

    [SerializeField] float grapplePower;
    [SerializeField] float grappleWindup;
    [SerializeField] float grappleTime;
    [SerializeField] float grappleCooldownTarget;
    [SerializeField] int baseGrapples;

    private void Awake()
    {
        Global.playerGrappling = this;
        rb = GetComponent<Rigidbody2D>();
        grapplingHookHolder = GameObject.FindGameObjectWithTag("GrapplingHolder").transform;

        grappleHook = Resources.Load<GameObject>("Prefabs/Grapple hook");
        grappleRoutine = null;
        remainingGrapples = baseGrapples;

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
        playerState = PlayerState.FREE;
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;
    }

    private void Update()
    {
        grappleCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(InputManager.Instance.GetInput("Grapple")) && grappleCooldown < 0 && remainingGrapples > 0)
        {
            Grapple();
        }
    }

    private void CreateGrappleObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject spawnHook = Instantiate<GameObject>(grappleHook, transform.position, Quaternion.identity, grapplingHookHolder);
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
        if (grappleRoutine == null)
        {
            remainingGrapples = baseGrapples;
        }
    }

    public void SetGrapples(int amount)
    {
        remainingGrapples = amount;
    }

    private void Grapple()
    {
        // Decreases remaining grapples the player can do before touching the ground again
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
            DisableGrappleObjects();
        }
        grappleRoutine = StartCoroutine(GrappleDuration());
        grappleCooldown = grappleCooldownTarget;
    }

    private IEnumerator GrappleDuration() // What happens while grappeling
    {
        // Gets input from player
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        // Spawns grappling hook from object pool, then moves it

        Vector2 grappleDirection = new Vector2(xInput, yInput).normalized;
        GameObject spawnHook = grapplingHooks.Dequeue();
        spawnHook.transform.position = transform.position;
        spawnHook.SetActive(true);
        rbG = spawnHook.GetComponent<Rigidbody2D>();
        rbG.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        // Waits
        yield return new WaitForSeconds(grappleWindup);

        // Sets player in the grappling state
        playerState = PlayerState.GRAPPLING;

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

        // Sets the player speed to what was before grappeling
        rb.velocity += maintainedVelocity;

        grapplingHooks.Enqueue(spawnHook);
        spawnHook.SetActive(false);
        grappleRoutine = null;
    }

    // If resetPlater is false then the players variables will not be reset
    private void CancelGrapple(bool resetPlayer)
    {
        // Resets all variables affected by grappling
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (playerState != PlayerState.GRAPPLING)
        {
            return;
        }
        bool resetPlayer = true;
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            resetPlayer = false;
            playerState = PlayerState.ATTACHED;
            rb.velocity = Vector2.zero;
        }
        CancelGrapple(resetPlayer);
    }

    public enum PlayerState
    {
        FREE, GRAPPLING, ATTACHED
    }
}
