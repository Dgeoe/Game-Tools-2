using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthBoost = 10; // Amount of health to add when player enters the trigger

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            // Find the PlayerHealth component on the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // If PlayerHealth is found, add health and destroy the pickup object
            if (playerHealth != null)
            {
                playerHealth.health += healthBoost;
                playerHealth.UpdateHealthUI(); // Update the health UI after picking up
                Destroy(gameObject); // Remove the health pickup from the game
                
            }
        }
    }
}
