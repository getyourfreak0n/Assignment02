using UnityEngine;
    
    /// <summary>
    /// Controls the player's movement, jumping, and animations in a Unity scene.
    /// Handles input from an InputHandler, applies physics for movement and jumping,
    /// and manages animator states for running and jumping.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// The Animator component for controlling player animations.
        /// </summary>
        Animator animator;
    
        /// <summary>
        /// The Rigidbody component for applying physics to the player.
        /// </summary>
        Rigidbody rb;
    
        /// <summary>
        /// Reference to the InputHandler for receiving input actions.
        /// </summary>
        [SerializeField] InputHandler inputHandler;
    
        /// <summary>
        /// The force applied when the player jumps.
        /// </summary>
        [SerializeField] float jumpForce = 10f;
        
        /// <summary>
        /// Layer mask for detecting ground collisions.
        /// </summary>
        public LayerMask groundLayer;
    
        /// <summary>
        /// The current input vector from the InputHandler.
        /// </summary>
        Vector2 input;
    
        /// <summary>
        /// The calculated movement vector based on input and speed.
        /// </summary>
        Vector3 movement;
    
        /// <summary>
        /// Timer for jump buffering to allow jumps slightly before landing.
        /// </summary>
        float jumpBufferTimer;                    
    
        /// <summary>
        /// Constant time window for jump buffering.
        /// </summary>
        const float jumpBufferTime = 0.15f;       
        
        /// <summary>
        /// Length of the ray used for ground checking.
        /// </summary>
        public float rayLength = 1.25f;
    
        /// <summary>
        /// Radius of the sphere used for ground checking.
        /// </summary>
        [SerializeField] float groundCheckRadius = 0.3f; 
        
        /// <summary>
        /// Speed at which the player moves.
        /// </summary>
        [SerializeField] float moveSpeed = 10f;
    
        /// <summary>
        /// Initializes the Rigidbody and Animator components.
        /// </summary>
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }
    
        /// <summary>
        /// Subscribes to the jump action when the script is enabled.
        /// </summary>
        void OnEnable() => inputHandler.jumpAction += handleJump;
    
        /// <summary>
        /// Unsubscribes from the jump action when the script is disabled.
        /// </summary>
        void OnDisable() => inputHandler.jumpAction -= handleJump;
    
        /// <summary>
        /// Updates input, movement, animation states, and rotation each frame.
        /// Also decrements the jump buffer timer.
        /// </summary>
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
    
        /// <summary>
        /// Applies movement velocity and handles jumping in the physics update.
        /// Checks if grounded and manages jump execution and animation states.
        /// </summary>
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
    
        /// <summary>
        /// Handles the jump input by setting the jump buffer timer.
        /// </summary>
        void handleJump()
        {
            jumpBufferTimer = jumpBufferTime;      
        }
        
        /// <summary>
        /// Checks if the player is grounded by casting a sphere at the ground check position.
        /// </summary>
        /// <returns>True if the player is grounded, false otherwise.</returns>
        bool isGrounded()
        {
            Vector3 spherePosition = transform.position + (Vector3.down * rayLength);
            return Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayer);
        }
    
        /// <summary>
        /// Draws gizmos for visualizing the ground check in the editor.
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 spherePosition = transform.position + (Vector3.down * rayLength);
            Gizmos.DrawLine(transform.position, spherePosition);
            Gizmos.DrawWireSphere(spherePosition, groundCheckRadius);
        }
    
        /// <summary>
        /// Applies jump force to the Rigidbody and sets the jumping animation state.
        /// </summary>
        void jumping()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
        }
    }