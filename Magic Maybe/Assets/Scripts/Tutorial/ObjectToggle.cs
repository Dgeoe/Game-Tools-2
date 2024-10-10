using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggle : MonoBehaviour
{
    [Header("Objects to Enable")]
    [SerializeField] public GameObject[] objectsToEnable; // Array of objects to enable

    [Header("Objects to Disable")]
    public GameObject[] objectsToDisable; // Array of objects to disable

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is tagged as "Player"
        {
            // Enable the objects in the array
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }

            // Disable the objects in the array
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
    }
}
