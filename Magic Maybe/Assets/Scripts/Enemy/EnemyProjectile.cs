using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage;

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure only to stick to the first target you hit
        //if (targetHit)
          //  return;
        //else
          //  targetHit = true;

        // check if you hit an enemy
        if(collision.gameObject.GetComponent<PlayerHealth>() != null)
        {
            PlayerHealth enemy = collision.gameObject.GetComponent<PlayerHealth>();

            enemy.TakeDamage(damage);

            // destroy projectile
            Destroy(gameObject);
        }

        // make sure projectile sticks to surface
 //       rb.isKinematic = true;

        // make sure projectile moves with target
   //     transform.SetParent(collision.transform);
    }
}