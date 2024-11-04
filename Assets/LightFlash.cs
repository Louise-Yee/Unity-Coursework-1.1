using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    private Light pointLight; // Reference to the point light

    // Duration for which the light should be enabled
    public float flashDuration = 0.1f;

    void Start()
    {
        pointLight = GetComponent<Light>(); // Get the Light component attached to this GameObject
        pointLight.enabled = false; // Initially disable the light
    }

    public void FlashLight()
    {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        pointLight.enabled = true; // Enable the light
        yield return new WaitForSeconds(flashDuration); // Wait for the specified duration
        pointLight.enabled = false; // Disable the light
    }
}
