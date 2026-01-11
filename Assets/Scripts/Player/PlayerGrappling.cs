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

    public void IncreaseGrapples(int amount)
    {
        remainingGrapples += amount;
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

        // Spawns and moves grappling hook

        Vector2 grappleDirection = new Vector2(xInput, yInput).normalized;
        GameObject spawnHook = grapplingHooks.Dequeue();//Instantiate<GameObject>(grappleHook, transform.position, Quaternion.identity);
        spawnHook.transform.position = transform.position;
        spawnHook.SetActive(true);
        rbG = spawnHook.GetComponent<Rigidbody2D>();
        rbG.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        // Waits
        yield return new WaitForSeconds(grappleWindup);

        // Sets speed to 0
        rbG.velocity = Vector2.zero;

        // Moves player
        rb.AddForce(grappleDirection * grapplePower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(grappleTime);

        
        PlayerMovement.canMove = true;
        rb.gravityScale = Global.playerGravityScale;

        // Resets players maintained velocity if not in the same direction as the grapple
        Vector2 normalizedMV = maintainedVelocity.normalized;
        //Debug.Log($"{normalizedMV.x}, {grappleDirection.x}, {maintainedVelocity.x}");
        maintainedVelocity.x = (normalizedMV.x == grappleDirection.x) ? maintainedVelocity.x : 0;
        maintainedVelocity.y = (normalizedMV.y == grappleDirection.y) ? maintainedVelocity.y : 0;

        // Sets the player speed to what was before grappeling
        rb.velocity += maintainedVelocity;

        //Destroy(spawnHook);
        grapplingHooks.Enqueue(spawnHook);
        spawnHook.SetActive(false);
        grappleRoutine = null;
    }
}
