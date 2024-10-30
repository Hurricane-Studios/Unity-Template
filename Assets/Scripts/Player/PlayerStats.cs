using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // === Current Stats ===
    [Header("Current Stats")]
    public float health;
    public float stamina;

    // === Max Stats ===
    [Header("Max Stats")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    // Reference to the Animator component for controlling animations
    private Animator animator;

    void Start()
    {
        // Initialize health and stamina to their maximum values at the start
        health = maxHealth;
        stamina = maxStamina;

        // Assign the Animator component to animator
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Round health and stamina to ensure they don't have decimal values
        health = Mathf.Round(health);
        stamina = Mathf.Round(stamina);

        // Check if health has reached zero and trigger death
        if (health <= 0)
        {
            ObjectDeath();
        }
    }

    // Method to apply damage to the player
    public void TakeDamage(float damageAmount)
    {
        // Reduce the player's health by the given damage amount
        health -= damageAmount;

        // Ensure health does not fall below 0 or exceed max health
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    // Method to regenerate stamina over time
    void RegenStamina()
    {
        // Only regenerate stamina if it's below the maximum limit
        if (stamina < maxStamina)
        {
            // Increase stamina gradually based on the stamina regeneration rate
            stamina += 5f * Time.deltaTime; // Adjust regeneration rate as necessary

            // Clamp the value to ensure stamina doesn't exceed max stamina
            stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        }
    }

    // Method to handle player's death
    void ObjectDeath()
    {
        // Trigger death animation
        animator.SetTrigger("Die");

        // Wait for 2 seconds and then destroy the game object
        Destroy(gameObject, 4f);
    }
}
