using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textMeshPro;        // The TextMeshPro component
    public float typeSpeed = 0.05f;     // Delay between each character (typewriter speed)
    public float disableAfter = 5f;     // Time after which the TextMeshPro object is disabled
    public float startDelay = 0f;       // Delay before the typewriter effect starts (optional)
    public AudioClip typeSoundClip;     // Audio clip for typewriter sound effect (optional)
    public float soundVolume = 0.5f;    // Volume for the typewriter sound effect

    private string fullText;            // Store the full text to display
    private bool isTyping = false;      // Tracks if the typing effect is running
    private AudioSource audioSource;    // Private audio source for playing the sound effect

    void Start()
    {
        // Save the full text and clear the display
        fullText = textMeshPro.text;
        textMeshPro.text = "";  

        // Create an AudioSource if an AudioClip is assigned
        if (typeSoundClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = typeSoundClip;
            audioSource.volume = soundVolume;
        }

        // Start the typewriter effect with the optional start delay
        StartCoroutine(StartTypewriterEffect());
    }

    // Coroutine to start the typewriter effect after a delay
    IEnumerator StartTypewriterEffect()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(TypeText());
    }

    // Coroutine for the typewriter effect
    IEnumerator TypeText()
    {
        isTyping = true;

        // Iterate through each character in the full text
        foreach (char letter in fullText.ToCharArray())
        {
            textMeshPro.text += letter;

            // Play typewriter sound if available
            if (audioSource != null)
            {
                audioSource.PlayOneShot(typeSoundClip);
            }

            // Wait for the typing speed before displaying the next character
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;

        // Disable the text after the specified time
        yield return new WaitForSeconds(disableAfter);
        textMeshPro.gameObject.SetActive(false);
    }
}

