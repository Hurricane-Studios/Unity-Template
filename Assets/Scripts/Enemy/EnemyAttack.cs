using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Damage dealt to the player during an attack
    public float damageAmount = 20f;

    // Reference to the Animator component for controlling animations
    private Animator animator;

    // Cooldown time between attacks to prevent continuous damage
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    // Reference to EnemyLogic for controlling sprint cooldown
    private EnemyLogic enemyLogic;

    void Start()
    {
        // Assign the Animator component to animator
        animator = GetComponentInChildren<Animator>();

        // Assign the EnemyLogic component to enemyLogic
        enemyLogic = GetComponent<EnemyLogic>();
    }

    // Called when the enemy's collider enters a trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with has the Player tag
        if (other.CompareTag("Player"))
        {
            // Get the PlayerStats component from the collided object
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            // If PlayerStats is found and enough time has passed since the last attack
            if (playerStats != null && Time.time >= lastAttackTime + attackCooldown)
            {
                // Play the attack animation
                animator.SetTrigger("Attack");

                // Deal damage to the player
                playerStats.TakeDamage(damageAmount);

                // Update the last attack time
                lastAttackTime = Time.time;

                // Prevent sprinting for the sprint cooldown period after attacking
                if (enemyLogic != null)
                {
                    enemyLogic.SetSprintCooldown();
                }
            }
        }
    }
}
