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

    // === Regeneration Rates ===
    [Header("Regeneration Rates")]
    public float healthRegenRate = 1f; // Health regenerated per second
    public float staminaRegenRate = 5f; // Stamina regenerated per second

    // === Health Regeneration Control ===
    [Header("Health Regeneration Control")]
    [SerializeField] private float lastDamageTime; // Timestamp for the last time damage was taken
    public float healthRegenDelay = 2f; // Delay in seconds before health starts to regenerate

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
        // Check if enough time has passed since taking damage to start health regeneration
        if (Time.time - lastDamageTime >= healthRegenDelay)
        {
            // Regenerate health if the player hasn't taken damage recently
            RegenHealth();
        }

        // Always regenerate stamina, regardless of taking damage
        RegenStamina();

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

        // Update the last damage time to the current time to start delay for regeneration
        lastDamageTime = Time.time;

        // Ensure health does not fall below 0 or exceed max health
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    // Method to regenerate health over time
    void RegenHealth()
    {
        // Only regenerate health if it's below the maximum limit
        if (health < maxHealth)
        {
            // Increase health gradually based on the health regeneration rate
            health += healthRegenRate * Time.deltaTime; // Use deltaTime to make regeneration smooth over time

            // Clamp the value to ensure health doesn't exceed max health
            health = Mathf.Clamp(health, 0f, maxHealth);
        }
    }

    // Method to regenerate stamina over time
    void RegenStamina()
    {
        // Only regenerate stamina if it's below the maximum limit
        if (stamina < maxStamina)
        {
            // Increase stamina gradually based on the stamina regeneration rate
            stamina += staminaRegenRate * Time.deltaTime; // Use deltaTime to make regeneration smooth over time

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
        Destroy(gameObject, 2f);
    }
}
