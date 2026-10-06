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
    private bool canJump = false;
    [SerializeField] private float coyoteTimerLength = 0.15f;
    private float coyoteTimeRemaining;
    private bool coyoteTimerStarted = false;
    private bool justJumped = false;

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
    }

    private void Update()
    {   
        // Sprite flipping
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        // Grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, groundLayer);

        if (isGrounded)
        {
            coyoteTimeRemaining = coyoteTimerLength;
        } else
        {
            coyoteTimeRemaining -= Time.deltaTime;
        }

        // Animations 
        animator.SetBool("isMoving", Mathf.Abs(moveInput.x) > 0.01f);
        
    }

    // Used to check grounded collider
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight));
    }
}
