# Unity-Template 3D URP (Universal Render Pipeline)
## Using Ver 2022.3.48f1 
## Join Our Discord if you need help or want to follow our other projects! https://discord.gg/XgS5Cuus7s
most likely, we will add more versions later on but this is where we will start
Also, anyone can contribute to this, this is to help newer devs get into the motions and give them a little more ground to stand on
For anyone who needs this, I recommend a code editor Visual Studio Code (VSC) or Visual Studio (VS)

## Script Explanations with Code Snippets
### PlayerMovement.cs
Start(): Initializes references to the player's Rigidbody for physics-based movement and the Animator for controlling character animations.
```
void Start()
{
    playerRb = GetComponent<Rigidbody>();
    animator = GetComponentInChildren<Animator>();
}
```
Update(): Manages player input for movement, sprinting, and jumping, and applies forces to move the player. It also handles ground detection and triggers corresponding animations for jumping and landing.
```
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
```
UpdateAnimations(Vector3 movement, bool isSprinting): Updates the Animator parameters to control the player's walking, sprinting, and grounded animations based on movement and sprinting state.
```
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
```

### CameraMovement.cs
Start(): Sets up the reference to the main camera and locks the cursor to the center of the screen to provide a first-person perspective.
```
void Start()
{
    // Get reference to the main camera in the scene
    mainCamera = Camera.main.transform;

    // Lock the cursor to the center of the screen and make it invisible
    Cursor.lockState = CursorLockMode.Locked;
}
```
Update(): Handles the rotation of the camera based on player mouse input, including limiting the up and down rotation for a realistic first-person view.
```
void Update()
{
    // Get mouse input from the Input Manager
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    // Rotate the camera up and down (Y-axis)
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit the up and down rotation

    // Apply rotation to the main camera
    mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    // Rotate the main camera left and right based on mouseX input
    transform.Rotate(Vector3.up * mouseX);
}
```

### PlayerStats.cs
Start(): Initializes the player's health and stamina to their maximum values and sets up references, including the Animator component.
```
void Start()
{
    health = maxHealth;
    stamina = maxStamina;
    animator = GetComponentInChildren<Animator>();
}
```
Update(): Rounds health and stamina to ensure they are whole numbers and checks if health has reached zero to trigger the death sequence.
```
void Update()
{
    health = Mathf.Round(health);
    stamina = Mathf.Round(stamina);

    if (health <= 0)
    {
        ObjectDeath();
    }
}
```
TakeDamage(float damageAmount): Decreases health by the specified damage amount and clamps health to ensure it remains between 0 and maxHealth.
```
public void TakeDamage(float damageAmount)
{
    health -= damageAmount;
    health = Mathf.Clamp(health, 0f, maxHealth);
}
```
RegenStamina(): Gradually regenerates stamina until it reaches its maximum value and ensures it does not exceed maxStamina.
```
void RegenStamina()
{
    if (stamina < maxStamina)
    {
        stamina += 5f * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }
}
```
ObjectDeath(): Plays the death animation and destroys the player object after 2 seconds.
```
void ObjectDeath()
{
    animator.SetTrigger("Die");
    Destroy(gameObject, 2f);
}
```

### EnemyStats.cs
Update(): Handles stamina regeneration if the enemy is in regeneration mode.
```
void Update()
{
    if (isRegeneratingStamina)
    {
        RegenStamina();
    }
}
```
UseStamina(): Decreases stamina while sprinting, starts regeneration if stamina hits zero, and returns a boolean indicating if stamina was successfully used.
```
public bool UseStamina()
{
    if (stamina > 0f)
    {
        stamina -= staminaUsageRate * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        if (stamina <= 0f)
        {
            isRegeneratingStamina = true;
        }

        return true;
    }
    else
    {
        isRegeneratingStamina = true;
        return false;
    }
}
```
RegenStamina(): Regenerates stamina over time until it reaches maximum and disables regeneration mode when fully regenerated.
```
private void RegenStamina()
{
    if (stamina < maxStamina)
    {
        stamina += staminaRegenRate * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    if (stamina >= maxStamina)
    {
        isRegeneratingStamina = false;
    }
}
```
CanSprint(): Checks if the enemy can sprint, based on whether it is not regenerating and the cooldown period has ended.
```
public bool CanSprint()
{
    return !isRegeneratingStamina && stamina == maxStamina;
}
```
SetSprintCooldown(float cooldownDuration): Sets a cooldown period for sprinting to prevent the enemy from sprinting after certain actions.
```
public void SetSprintCooldown(float cooldownDuration)
{
    sprintCooldownEndTime = Time.time + cooldownDuration;
}
```

### EnemyLogic.cs
Start(): Initializes references to NavMeshAgent, EnemyStats, and Animator, and finds the player target based on the "Player" tag.
```
void Start()
{
    agent = GetComponent<NavMeshAgent>();
    enemyStats = GetComponent<EnemyStats>();
    animator = GetComponentInChildren<Animator>();
    agent.speed = walkSpeed;

    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        target = player.transform;
    }
}
```
Update(): Controls enemy movement by setting the agent's destination, switching between walking and sprinting based on stamina, and updates the Animator parameters accordingly.
```
void Update()
{
    if (target != null)
    {
        agent.SetDestination(target.position);

        if (Time.time >= sprintCooldownEndTime && enemyStats.CanSprint())
        {
            if (enemyStats.UseStamina())
            {
                agent.speed = sprintSpeed;
                animator.SetBool("IsSprinting", true);
            }
        }
        else
        {
            agent.speed = walkSpeed;
            animator.SetBool("IsSprinting", false);
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
```
SetSprintCooldown(): Sets a cooldown timer to prevent the enemy from sprinting after specific events, such as an attack.
```
public void SetSprintCooldown()
{
    sprintCooldownEndTime = Time.time + sprintCooldownDuration;
}
```

### EnemyAttack.cs
Start(): Initializes references to Animator and EnemyLogic components.
```
void Start()
{
    animator = GetComponentInChildren<Animator>();
    enemyLogic = GetComponent<EnemyLogic>();
}
```
OnTriggerEnter(Collider other): When the enemy collides with an object tagged as "Player," it plays the attack animation, deals damage to the player, and initiates the sprint cooldown.
```
void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats != null && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            playerStats.TakeDamage(damageAmount);
            lastAttackTime = Time.time;

            if (enemyLogic != null)
            {
                enemyLogic.SetSprintCooldown();
            }
        }
    }
}
```
