using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Player movement
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    private Vector2 moveInput;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Grounded Logic
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckWidth = 0.2f;
    [SerializeField] private float groundCheckHeight = 0.2f;
    private bool isGrounded;


    // Coyote Time
    [SerializeField] private float coyoteTimerLength = 0.15f;
    private float coyoteTimeRemaining;

    // Fluid movement of character  
    [SerializeField] private float riseGravity = 2f;
    [SerializeField] private float fallGravity = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && coyoteTimeRemaining > 0f)
        {
            coyoteTimeRemaining = 0f;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            ); 
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y
        );

        // Check if player is moving vertically
        if (rb.linearVelocity.y < 0)
        {
            // Falling
            rb.gravityScale = fallGravity;
        }
        else
        {
            // Rising
            rb.gravityScale = riseGravity;
        }
    }

    private void Update()
    {   
        // Sprite flipping
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (moveInput.x < 0)
        {   
            spriteRenderer.flipX = true;
        }
            
        // Grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, groundLayer);

        // When to set jump animation
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            coyoteTimeRemaining = coyoteTimerLength;
        } else
        {
            coyoteTimeRemaining -= Time.deltaTime;
        }

        // Run animation if player is moving 
        animator.SetBool("isMoving", Mathf.Abs(moveInput.x) > 0.01f);
    }

    // Used to check grounded collider
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight));
    }
}
