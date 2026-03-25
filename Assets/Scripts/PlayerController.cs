using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] float jumpForce = 10f;
    
    public LayerMask groundLayer;
    Vector2 input;
    Vector3 movement;
    float jumpBufferTimer;                    
    const float jumpBufferTime = 0.15f;       
    
    public float rayLength = 1.25f;
    [SerializeField] float groundCheckRadius = 0.3f; 
    
    [SerializeField] float moveSpeed = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void OnEnable() => inputHandler.jumpAction += handleJump;
    void OnDisable() => inputHandler.jumpAction -= handleJump;

    void Update()
    {
        input = inputHandler.moveVector;
        movement = new Vector3(input.x * moveSpeed, 0, input.y * moveSpeed);
        
        bool isMoving = input.magnitude > 0.1f;
        animator.SetBool("isRunning", isMoving);
       
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
        
        if (jumpBufferTimer > -1f) 
        {
            jumpBufferTimer -= Time.deltaTime;     
        }
    }

    void FixedUpdate()
    {
        bool grounded = isGrounded();
        
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        if (jumpBufferTimer > 0f && grounded)  
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); 
            jumping();
            jumpBufferTimer = 0f;  
        }
        else if (grounded && rb.linearVelocity.y <= 0.1f)
        {
            animator.SetBool("isJumping", false);
        }
    }

    void handleJump()
    {
        jumpBufferTimer = jumpBufferTime;      
    }
    
    bool isGrounded()
    {
        Vector3 spherePosition = transform.position + (Vector3.down * rayLength);
        return Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 spherePosition = transform.position + (Vector3.down * rayLength);
        Gizmos.DrawLine(transform.position, spherePosition);
        Gizmos.DrawWireSphere(spherePosition, groundCheckRadius);
    }

    void jumping()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("isJumping", true);
    }
}