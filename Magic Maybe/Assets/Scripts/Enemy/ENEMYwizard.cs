using UnityEngine;

public class ENEMYwizard : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;  // Fire point for projectiles
    public Transform targetPoint;  // New target point for shooting
    public float patrolRange = 10f;
    public float attackRange = 5f;
    public float moveSpeed = 2f;
    public float fireRate = 1.5f;  // Time between projectile shots
    public float projectileForce = 15f;  // Adjustable force for the projectile in the inspector
    private float nextFireTime;

    private Transform playerTransform;  // To store the player's transform
    private bool canSeePlayer = true; // Controls if enemy can patrol or attack
    private enum State { Idle, Patrolling, Attacking }
    private State currentState = State.Idle;

    void Start()
    {
        // Find the player's transform at runtime using the tag "Player"
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!canSeePlayer)
        {
            currentState = State.Idle; // Set to Idle when the player is invisible
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Rotate enemy to always face the player
        RotateToFacePlayer();

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer < patrolRange)
                {
                    currentState = State.Patrolling;
                }
                break;

            case State.Patrolling:
                if (distanceToPlayer < attackRange)
                {
                    currentState = State.Attacking;
                }
                else
                {
                    MoveTowardsPlayer();
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Patrolling;
                }
                else
                {
                    AttackPlayer();
                }
                break;
        }
    }

    void RotateToFacePlayer()
    {
        // Get direction to player and set the enemy rotation
        //Vector3 direction = (playerTransform.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);  // Smooth rotation
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void AttackPlayer()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        // Instantiate projectile from the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Shoot towards the custom target point instead of directly at the player's transform
        Vector3 direction = (targetPoint.position - firePoint.position).normalized;
        rb.velocity = direction * projectileForce;  // Apply adjustable projectile force

        Destroy(projectile, 3f);  // Destroy projectile after 3 seconds
    }

    // Method to handle player's visibility state
    public void SetPlayerVisibility(bool isVisible)
    {
        canSeePlayer = isVisible;
    }
}
