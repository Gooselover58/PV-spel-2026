using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Entity
{
    private Rigidbody2D rb;
    private Transform groundCheckTrans;

    public static bool canMove;
    private bool isJumping;

    public static float xInput;
    public static float yInput;

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
        Global.playerMovement = this;

        canMove = true;
        isJumping = false;

        coyoteTime = 0;
        jumpBufferTime = 0;
    }

    private void Start()
    {
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
        
        if (Input.GetKeyDown(InputManager.Instance.GetInput("Jump")))
        {
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
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(xInput * moveSpeed, rb.velocity.y);
            rb.velocity = movement;
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
            return true;
        }
        return false;
    }

    private void FreezeMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckForHazards(col);
        CheckForTransition(col);
    }

    private void CheckForHazards(Collider2D col)
    {
        if (col.gameObject.CompareTag("Hazard"))
        {
            GameManager.Instance.RespawnPlayer();
        }
    }

    private void CheckForTransition(Collider2D col)
    {
        RoomTransition transition = col.GetComponent<RoomTransition>();
        if (transition != null && canMove)
        {
            FreezeMovement();
            transition.Transition();
        }
    }
}
