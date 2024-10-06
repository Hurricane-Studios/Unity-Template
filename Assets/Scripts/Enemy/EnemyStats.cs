using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Current stamina of the enemy
    public float stamina = 100f;

    // Maximum stamina value
    public float maxStamina = 100f;

    // Stamina regeneration rate per second
    public float staminaRegenRate = 10f;

    // Stamina usage rate while sprinting per second
    public float staminaUsageRate = 20f;

    // Whether the enemy is regenerating stamina
    private bool isRegeneratingStamina = false;

    void Update()
    {
        // Regenerate stamina if the enemy is not using it or it has reached zero
        if (isRegeneratingStamina)
        {
            RegenStamina();
        }
    }

    // Method to use stamina while sprinting
    public bool UseStamina()
    {
        if (stamina > 0f)
        {
            stamina -= staminaUsageRate * Time.deltaTime;

            // Clamp stamina to ensure it doesn't go below zero
            stamina = Mathf.Clamp(stamina, 0f, maxStamina);

            // If stamina hits zero, start regenerating
            if (stamina <= 0f)
            {
                isRegeneratingStamina = true;
            }

            return true; // Stamina was used successfully
        }
        else
        {
            isRegeneratingStamina = true;
            return false; // Not enough stamina to use
        }
    }

    // Method to regenerate stamina over time
    private void RegenStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;

            // Clamp stamina to ensure it doesn't exceed max stamina
            stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        }

        // If stamina is fully regenerated, stop regenerating
        if (stamina >= maxStamina)
        {
            isRegeneratingStamina = false;
        }
    }

    // Method to check if the enemy can sprint
    public bool CanSprint()
    {
        return !isRegeneratingStamina && stamina == maxStamina;
    }
}
