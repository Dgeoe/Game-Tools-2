using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float shotPower = 30f;
    [SerializeField] private float stopVelocity = 0.05f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float dashForce = 10f; // Force applied when dashing
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip splashSound;
    [SerializeField] private AudioClip shootSound;

    [SerializeField] private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAiming;
    private bool isGrounded; // Track if the player is grounded
    private Rigidbody rigidBody;
    private Vector3 lastPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        isAiming = false;
        lineRenderer.enabled = false;
        lastPosition = transform.position; // Initial position
    }

    private void Update()
    {
        ProcessJumpHaltDash();
    }

    private void FixedUpdate()
    {
        if (rigidBody.linearVelocity.magnitude < stopVelocity)
        {
            Stop();
        }
        ProcessAim();
    }

    private void OnMouseDown()
    {
        if (isIdle)
        {
            isAiming = true;
        }
    }

    private void ProcessAim()
    {
        if (!isAiming || !isIdle)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();
        if (!worldPoint.HasValue)
        {
            return;
        }

        DrawLine(worldPoint.Value);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(worldPoint.Value);
        }
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;

        // Save the last position before shooting
        lastPosition = transform.position;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        Vector3 currentVelocity = rigidBody.linearVelocity;

        if (Vector3.Dot(currentVelocity.normalized, direction) < 0)
        {
            strength *= 2; // Double the power if opposite
        }

        rigidBody.AddForce(-direction * strength * shotPower);

        // Play shooting sound
        PlaySound(shootSound);

        isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = 
        {
            transform.position,
            worldPoint
        };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        return null;
    }

    // Handle collision with "Splash" and "Coin", as well as grounding logic
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Splash"))
        {
            transform.position = lastPosition; // Reset to last position
            rigidBody.linearVelocity = Vector3.zero; // Stop movement
            PlaySound(splashSound);
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            // Play coin sound
            PlaySound(coinSound);
            
            // Halt velocity
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;

            // Disable the coin object
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Player is grounded
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Player is no longer grounded
        }
    }

    // Handle jump, halt, and dash movement
    private void ProcessJumpHaltDash()
    {
        // Jump if grounded and pressing space
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Jump
            isGrounded = false; // Player is now airborne
        }

        // Halt movement with "S"
        if (Input.GetKeyDown(KeyCode.S))
        {
            rigidBody.linearVelocity = Vector3.zero; // Halt movement
            rigidBody.angularVelocity = Vector3.zero;
        }

        // Dash in current direction with "W"
        if (Input.GetKeyDown(KeyCode.W))
        {
            Dash();
        }

        // Reset to last position with "Q"
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = lastPosition;
            rigidBody.linearVelocity = Vector3.zero; // Stop movement
        }
    }

    // Dash in the current movement direction
    private void Dash()
    {
        if (rigidBody.linearVelocity.magnitude > 0)
        {
            Vector3 dashDirection = rigidBody.linearVelocity.normalized; // Keep the current direction
            rigidBody.AddForce(dashDirection * dashForce, ForceMode.Impulse); // Apply dash force
        }
    }

    // Method to play sound effects
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

