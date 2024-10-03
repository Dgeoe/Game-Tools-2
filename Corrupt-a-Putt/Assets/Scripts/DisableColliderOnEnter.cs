using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderOnEnter : MonoBehaviour
{
    [SerializeField] private Collider targetCollider;
    [SerializeField] private Rigidbody playerRigidbody; // Reference to the player's Rigidbody
    [SerializeField] private GameObject[] objectsToEnable; // Array of objects to enable
    [SerializeField] private GameObject[] objectsToDisable; // Array of objects to disable

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable the target collider
            targetCollider.enabled = false;

            // Set the player's velocity to zero
            if (playerRigidbody != null)
            {
                playerRigidbody.linearVelocity = Vector3.zero;
                playerRigidbody.angularVelocity = Vector3.zero; // Stops any rotational movement as well
            }

            // Enable the selected objects
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }

            // Disable the selected objects
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Re-enable the target collider
            targetCollider.enabled = true;
        }
    }
}
