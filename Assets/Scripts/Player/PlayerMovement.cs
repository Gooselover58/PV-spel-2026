using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Entity
{
    private Rigidbody2D rb;
    private Transform groundCheckTrans;
    private PlayerGrappling playerGrappling;

    public static bool canMove;
    private bool isJumping;

    public static Vector2 movementInput;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCancelSpeed;
    [SerializeField] float groundCheckSize;
    [SerializeField] float coyoteJump;
    [SerializeField] float jumpBuffer;
    [SerializeField] float jumpCooldown;
    [SerializeField] float freeInputTime;
    [SerializeField] LayerMask groundLayer;

    private float coyoteTime;
    private float jumpBufferTime;
    private float jumpCooldownTime;
    private float inputDuration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheckTrans = transform.GetChild(0);
        Global.playerTrans = transform;
        Global.playerRb = rb;
        Global.playerMovement = this;

        canMove = true;
        isJumping = false;

        coyoteTime = 0f;
        jumpBufferTime = 0f;
        inputDuration = 0f;
    }

    private void Start()
    {
        playerGrappling = Global.playerGrappling;
        groundLayer = Global.groundLayer;
    }

    private void OnEnable()
    {
        canMove = true;
        rb.gravityScale = Global.playerGravityScale;
    }

    private void Update()
    {
        if (IsGrounded())
        {
            coyoteTime = coyoteJump;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        bool pressedJump = false;

        if (Input.GetKeyDown(InputManager.Instance.GetInput("Jump")))
        {
            pressedJump = true;
            jumpBufferTime = jumpBuffer;
        }
        else
        {
            jumpBufferTime -= Time.deltaTime;
        }

        if (!Input.GetKey(InputManager.Instance.GetInput("Jump")) && rb.velocity.y > 0 && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - jumpCancelSpeed);
        }
        else if (rb.velocity.y <= 0 && isJumping)
        {
            isJumping = false;
        }

        jumpCooldownTime -= Time.deltaTime;

        if (pressedJump && playerGrappling.playerState == PlayerGrappling.PlayerState.ATTACHED)
        {
            Jump();
            playerGrappling.ResetPlayer();
        }
        else if (jumpBufferTime > 0 && coyoteTime > 0)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        movementInput = InputManager.Instance.GetMovement();

        // Counts how long the player has been holding down while attached
        if (movementInput.y < 0 && playerGrappling.playerState == PlayerGrappling.PlayerState.ATTACHED)
        {
            inputDuration += Time.deltaTime;
        }
        else
        {
            inputDuration = 0;
        }

        // If the player has been making inputs for long enough while attached, free them
        if (inputDuration > freeInputTime)
        {
            playerGrappling.ResetPlayer();
        }

        // Performs player movement if possible
        if (canMove)
        {
            // Slows movement speed the further away velocity is from 0
            float normalizeMod = (-movementInput.x * rb.velocity.x);
            normalizeMod *= (movementInput.x > 0) ? 1 : -1;
            normalizeMod = Mathf.Clamp(normalizeMod, -moveSpeed, moveSpeed);

            // Moves player
            Vector2 movement = new Vector2(movementInput.x * moveSpeed + normalizeMod, 0);
            rb.AddForce(movement, ForceMode2D.Impulse);
        }
    }

    public override void BoostEntity(Vector2 dir, float power)
    {
        if (!isJumping)
        {
            rb.AddForce(dir * power, ForceMode2D.Impulse);
            playerGrappling.ResetGrapples();
        }
    }

    private void Jump()
    {
        coyoteTime = 0;
        jumpBufferTime = 0;
        jumpCooldownTime = jumpCooldown;
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        Collider2D col = Physics2D.OverlapCircle(groundCheckTrans.position, groundCheckSize, groundLayer);
        if (col != null && jumpCooldownTime <= 0)
        {
            isJumping = false;
            playerGrappling.ResetGrapples();
            return true;
        }
        return false;
    }

    public void FreezeMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
    }
}
