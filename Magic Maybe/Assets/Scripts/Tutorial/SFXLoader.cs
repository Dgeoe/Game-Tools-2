using UnityEngine;

public class SFXLoader : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundEffects; // Array to store sound effects
    public float timeBeforeSwitch = 5f; // Time to switch sound effect

    private void Start()
    {
        if (soundEffects.Length >= 2 && audioSource != null)
        {
            audioSource.clip = soundEffects[0]; // Play the first sound effect
            audioSource.Play();
            Invoke("PlayNextSound", timeBeforeSwitch); // Switch after the specified time
        }
        else
        {
            Debug.LogWarning("Please assign at least two sound effects and an AudioSource.");
        }
    }

    private void PlayNextSound()
    {
        if (soundEffects.Length > 1)
        {
            audioSource.clip = soundEffects[1]; // Play the second sound effect
            audioSource.Play();
        }
    }
}
