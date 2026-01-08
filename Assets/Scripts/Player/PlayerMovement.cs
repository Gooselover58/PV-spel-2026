using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Entity
{
    private Rigidbody2D rb;
    private Transform groundCheckTrans;

    private bool canMove;
    private bool isJumping;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCancelSpeed;
    [SerializeField] float groundCheckSize;
    [SerializeField] float coyoteJump;
    [SerializeField] float jumpBuffer;
    [SerializeField] float jumpCooldown;
    [SerializeField] LayerMask groundLayer;

    private float coyoteTime;
    private float jumpBufferTime;
    private float jumpCooldownTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheckTrans = transform.GetChild(0);
        Global.playerTrans = transform;
        Global.playerRb = rb;

        canMove = true;
        isJumping = false;

        coyoteTime = 0;
        jumpBufferTime = 0;
    }

    private void Start()
    {
        groundLayer = Global.groundLayer;
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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTime = jumpBuffer;
        }
        else
        {
            jumpBufferTime -= Time.deltaTime;
        }

        if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 0 && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - jumpCancelSpeed);
        }
        else if (rb.velocity.y <= 0 && isJumping)
        {
            isJumping = false;
        }

        jumpCooldownTime -= Time.deltaTime;

        if (jumpBufferTime > 0 && coyoteTime > 0)
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
        if (canMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            Vector2 movement = new Vector2(x * moveSpeed, rb.velocity.y);
            rb.velocity = movement;
        }
    }

    private void Jump()
    {
        if (!IsGrounded())
        {
            return;
        }
        coyoteTime = 0;
        jumpBufferTime = 0;
        jumpCooldownTime = jumpCooldown;
        isJumping = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        Collider2D col = Physics2D.OverlapCircle(groundCheckTrans.position, groundCheckSize, groundLayer);
        if (col != null && jumpCooldownTime <= 0)
        {
            isJumping = false;
            return true;
        }
        return false;
    }
}
