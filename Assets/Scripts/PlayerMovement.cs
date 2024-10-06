using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables to control movement speed and jump force
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;

    // Reference to the character's Rigidbody for physics-based movement
    private Rigidbody playerRb;

    // Reference to the Animator component for controlling animations
    private Animator animator;

    // Variables to manage player input and state
    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;

    // Track the player's grounded status to determine when they land
    private bool wasGrounded;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the Rigidbody component to playerRb
        playerRb = GetComponent<Rigidbody>();

        // Assign the Animator component to animator
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input from the Input Manager (Horizontal and Vertical axes)
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Determine if the player is grounded (using a simple raycast check)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Handle sprinting and apply appropriate movement speed
        bool isSprinting = Input.GetButton("Sprint");
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Calculate movement vector and apply to the player's Rigidbody
        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        playerRb.velocity = new Vector3(movement.x * currentSpeed, playerRb.velocity.y, movement.z * currentSpeed);

        // Handle jumping if the player presses the Jump button and is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump"); // Trigger jump animation
        }

        // Update the Animator parameters to control animations
        UpdateAnimations(movement, isSprinting);

        // Handle landing animation if the player has just landed
        if (!wasGrounded && isGrounded)
        {
            animator.SetTrigger("Land"); // Trigger landing animation
        }

        // Update the previous grounded status
        wasGrounded = isGrounded;
    }

    // Update the Animator to play the appropriate animation based on movement state
    void UpdateAnimations(Vector3 movement, bool isSprinting)
    {
        // Calculate the movement magnitude (ignoring the Y-axis)
        float movementMagnitude = new Vector3(movement.x, 0f, movement.z).magnitude;

        // Set the "Speed" parameter to control walk/run animation
        animator.SetFloat("Speed", movementMagnitude);

        // Set the "IsSprinting" parameter to true or false based on sprint state
        animator.SetBool("IsSprinting", isSprinting);

        // Set the "IsGrounded" parameter to update grounded status in the Animator
        animator.SetBool("IsGrounded", isGrounded);
    }
}
