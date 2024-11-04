using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX; // Import the Visual Effect namespace

public class ShootingScript : MonoBehaviour
{
    public Camera playerCamera; // Reference to the camera for aiming
    public VisualEffect muzzleFlash; // Muzzle flash effect using VisualEffect
    public GameObject impactEffect; // Impact effect on hit
    public LightFlash lightFlash;

    [SerializeField]
    private AudioSource gunShotAudio; // Reference to the AudioSource

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) // Fire1 is usually mapped to left mouse button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (gunShotAudio != null)
        {
            gunShotAudio.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found for gunshot sound.");
        }
        // Show muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        else
        {
            Debug.Log("no muzzle flash");
        }

        if (lightFlash != null)
        {
            lightFlash.FlashLight(); // Call the method to flash the light
        }

        // Raycast from the center of the screen
        RaycastHit hit;
        if (
            Physics.Raycast(
                playerCamera.transform.position,
                playerCamera.transform.forward,
                out hit
            )
        )
        {
            Debug.Log(hit.transform.name); // Log the name of the object hit

            // Check if the object hit has a "EnemiesAI" component to apply damage
            EnemiesAI enemy = hit.transform.GetComponent<EnemiesAI>();
            if (enemy != null)
            {
                enemy.takeDamageFromPlayer(); // Call takeDamageFromPlayer() on the correct target
            }

            // Instantiate impact effect at the hit point
            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(
                    impactEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
                Destroy(impactGO, 2f); // Destroy impact effect after 2 seconds
            }
        }
    }
}
