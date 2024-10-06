using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    // Reference to the NavMeshAgent component for pathfinding
    private NavMeshAgent agent;

    // Reference to EnemyStats for stamina management
    private EnemyStats enemyStats;

    // Walking and sprinting speed
    public float walkSpeed = 3.5f;
    public float sprintSpeed = 7f;

    // Reference to the Animator component for controlling animations
    private Animator animator;

    // Track the previous position of the enemy for calculating distance walked
    private Vector3 lastPosition;

    // Target to follow, assigned based on the Player tag
    private Transform target;

    // Sprint cooldown variables
    public float sprintCooldownDuration = 2f;
    private float sprintCooldownEndTime = 0f;

    void Start()
    {
        // Assign the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Assign the EnemyStats component
        enemyStats = GetComponent<EnemyStats>();

        // Assign the Animator component
        animator = GetComponentInChildren<Animator>();

        // Set initial agent speed
        agent.speed = walkSpeed;

        // Initialize lastPosition to the current position
        lastPosition = transform.position;

        // Find the player GameObject by tag and assign its transform to target
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        // If the target is assigned, set the agent's destination
        if (target != null)
        {
            agent.SetDestination(target.position);

            // Check if the sprint cooldown has ended and if the enemy can sprint
            if (Time.time >= sprintCooldownEndTime && enemyStats.CanSprint())
            {
                if (enemyStats.UseStamina())
                {
                    // Set sprint speed
                    agent.speed = sprintSpeed;
                    animator.SetBool("IsSprinting", true);
                }
            }
            else
            {
                // Set walk speed if enemy cannot sprint
                agent.speed = walkSpeed;
                animator.SetBool("IsSprinting", false);
            }

            // Update walking/running animation speed based on agent velocity
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    // Method to set sprint cooldown after an attack
    public void SetSprintCooldown()
    {
        sprintCooldownEndTime = Time.time + sprintCooldownDuration;
    }
}
