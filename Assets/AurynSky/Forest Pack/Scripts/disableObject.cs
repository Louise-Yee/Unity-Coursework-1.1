using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableObject : MonoBehaviour

{
    // Reference to the GameObject you want to disable
    public GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        // Check for the condition you want (e.g., Player entering the trigger)
        if (other.CompareTag("Player")) // Make sure the "Player" tag is applied to the player object
        {
            // Disable the target GameObject
            targetObject.SetActive(false);
        }
    }
}

