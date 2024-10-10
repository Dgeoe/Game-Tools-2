using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kino;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int health;

    public DigitalGlitch GlitchEffect;
    private int initialHealth; // Store the initial health value

    public AudioSource audioSource2; // Reference to the AudioSource on the player object
    public AudioClip[] soundEffect;

    [Header("UI Elements")]
    public TextMeshProUGUI healthText; // Reference to the TMP UI element

    private Vector3 originalPosition; // To store the original position of the object

    private void Start()
    {
        // Store the initial health and original position
        initialHealth = health;
        originalPosition = transform.position;

        UpdateHealthUI(); // Initialize the health display
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthUI(); // Update the UI after taking damage

        GlitchEffect.intensity = 0.1f;

        // Play "Throw" sound effect (index 1 in the array)
        if (soundEffect.Length > 1 && soundEffect[3] != null)
        {
            audioSource2.PlayOneShot(soundEffect[3]);
        }

        // Reset the glitch effect intensity to zero after 1 second
        Invoke(nameof(ResetGlitchEffect), 1f);


        if (health <= 0)
            ResetPosition(); // Reset the object's position instead of destroying it
    }

    // Method to reset glitch effect intensity
    private void ResetGlitchEffect()
    {
        GlitchEffect.intensity = 0f;
        //GlitchEffect2.intensity = 0f; // Uncomment if using analog glitch
    }

    // Method to reset the object's position and health
    private void ResetPosition()
    {
        transform.position = originalPosition; // Reset to the original spawn position
        health = initialHealth; // Reset health to initial value
        UpdateHealthUI(); // Update the health UI to show reset health
    }

    // Make this method public so other scripts can access it
    public void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString(); // Display current health in UI
        }
    }
}

