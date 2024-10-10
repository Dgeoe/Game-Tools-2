using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttacks : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public GameObject objectToEnable; // The object to enable/disable on "Get" and "Throw"
    public RawImage crosshair; // Reference to the crosshair RawImage
    public AudioSource audioSource; // Reference to the AudioSource on the player object
    public Animator animator; // Reference to the Animator component for animations

    [Header("Settings")]
    public int totalThrows = 0; // Player starts with 0 throws
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;
    bool readyToThrow;
    private bool insideEnemyTrigger = false; // Track if player is inside enemy trigger
    private GameObject currentEnemy; // Store reference to the current enemy

    [Header("Audio Clips")]
    public AudioClip[] soundEffects; // Array for attaching sound effects (0 = Get, 1 = Throw, 2 = Nose)

    // Input system action references
    private PlayerInput playerInput;
    private InputAction shootAction;
    private InputAction getAction;
    private InputAction noseAction; // For "Got your nose" input action
    private InputAction invisibleAction; // For "Invisible" input action

    private bool isInvisible = false; // Track invisibility state

    private void Awake()
    {
        // Initialize the PlayerInput component
        playerInput = GetComponent<PlayerInput>();

        // Assign the "Shoot", "Get", "Nose", and "Invisible" actions from the action map
        shootAction = playerInput.actions["Shoot"];
        getAction = playerInput.actions["Get"];
        noseAction = playerInput.actions["Got your nose"];
        invisibleAction = playerInput.actions["Invisible"];

        // Subscribe to the performed callbacks of the actions
        shootAction.performed += ctx => OnShoot();
        getAction.performed += ctx => OnGet();
        noseAction.performed += ctx => OnNose();
        invisibleAction.performed += ctx => ToggleInvisibility();
    }

    private void Start()
    {
        readyToThrow = true;
        objectToEnable.SetActive(false); // Make sure the object is initially disabled

        // Ensure crosshair is white at the start
        crosshair.color = Color.white;

        // Set the default animation to Idle
        animator.SetTrigger("Idle");
    }

    private void OnGet()
    {
        // Check if player is inside enemy trigger to allow "Get"
        if (insideEnemyTrigger)
        {
            totalThrows++; // Increase throw count
            objectToEnable.SetActive(true); // Enable object when "Get" is pressed

            // Play "Get" sound effect (index 0 in the array)
            if (soundEffects.Length > 0 && soundEffects[0] != null)
            {
                audioSource.PlayOneShot(soundEffects[0]);
            }

            // Trigger "Get" animation
            animator.SetTrigger("Get");
        }
    }

    private void OnShoot()
    {
        if (readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Make projectile shoot towards crosshair
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // Force added so you always throw in the direction you are looking
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        // Impulse only adds force once (not continuously)
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // Disable the object after the player throws
        objectToEnable.SetActive(false);

        // Play "Throw" sound effect (index 1 in the array)
        if (soundEffects.Length > 1 && soundEffects[1] != null)
        {
            audioSource.PlayOneShot(soundEffects[1]);
        }

        // Trigger "Throw" animation
        animator.SetTrigger("Throw");

        // Destroy the projectile after 3 seconds
        Destroy(projectile, 3f);

        // Implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void OnNose()
{
    // Check if player is inside enemy trigger to allow "Got your nose"
    if (insideEnemyTrigger && currentEnemy != null)
    {
        // Trigger "Nose" animation
        animator.SetTrigger("Nose");

        // Play "Nose" sound effect (index 2 in the array)
        if (soundEffects.Length > 2 && soundEffects[2] != null)
        {
            audioSource.PlayOneShot(soundEffects[2]);
        }

        // Destroy the enemy
        Destroy(currentEnemy);

        // Set animation state back to "Idle" after "Nose"
        animator.SetTrigger("Idle");
    }
}


    private void ResetThrow()
    {
        readyToThrow = true;

        // Return to Idle animation after throw cooldown
        animator.SetTrigger("Idle");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player entered an enemy's trigger collider
        if (other.CompareTag("Enemy"))
        {
            insideEnemyTrigger = true;
            currentEnemy = other.gameObject; // Store reference to the current enemy
            crosshair.color = Color.yellow; // Change crosshair to yellow when inside enemy trigger
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if player exited an enemy's trigger collider
        if (other.CompareTag("Enemy"))
        {
            insideEnemyTrigger = false;
            currentEnemy = null; // Clear reference to the current enemy
            crosshair.color = Color.white; // Change crosshair back to white after leaving the enemy trigger
        }
    }

    private void ToggleInvisibility()
    {
        isInvisible = !isInvisible; // Toggle invisibility state

        // Trigger the "Peekaboo" animation when invisible, "Idle" otherwise
        if (isInvisible)
        {
            animator.SetTrigger("Peekaboo");
        }
        else
        {
            animator.SetTrigger("Idle");
        }

        // Notify the enemies about the player's invisibility
        foreach (ENEMYwizard enemy in FindObjectsOfType<ENEMYwizard>())
        {
            enemy.SetPlayerVisibility(!isInvisible); // Send the visibility state to the enemies
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the events when the object is disabled
        shootAction.performed -= ctx => OnShoot();
        getAction.performed -= ctx => OnGet();
        noseAction.performed -= ctx => OnNose();
        invisibleAction.performed -= ctx => ToggleInvisibility();
    }
}
