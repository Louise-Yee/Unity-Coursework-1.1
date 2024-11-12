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

    private TutorialManager tutorialManager;
    private PlayerScore playerScore; // Reference to PlayerScore for updating the score

    [SerializeField]
    private AudioSource gunShotAudio; // Reference to the AudioSource

    void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        playerScore = FindObjectOfType<PlayerScore>(); // Get the PlayerScore component from the scene
        if (tutorialManager == null)
        {
            // Debug.LogError("TutorialManager not found in scene");
        }
        if (playerScore == null)
        {
            // Debug.LogError("PlayerScore not found in the scene. Make sure it's attached to a gameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialManager.isShootUnlocked)
        {
            if (Input.GetMouseButtonDown(0)) // Fire1 is usually mapped to left mouse button
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        PlayerOptionMenu playerOptionMenu = GetComponent<PlayerOptionMenu>();

        if (playerOptionMenu.isGameOver || playerOptionMenu.isPaused)
        {
            return;
        }
        if (gunShotAudio != null)
        {
            gunShotAudio.Play();
        }
        else
        {
            // Debug.LogWarning("No AudioSource found for gunshot sound.");
        }
        // Show muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        else
        {
            // Debug.Log("No muzzle flash");
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
            // Debug.Log(hit.transform.name); // Log the name of the object hit

            TutorialEnemyBehavior tutorialEnemy =
                hit.transform.GetComponent<TutorialEnemyBehavior>();
            if (tutorialEnemy != null)
            {
                // Call the Die method to destroy the enemy
                tutorialEnemy.Die();

                // Optionally, add score or effects here
                if (playerScore != null)
                {
                    playerScore.AddScore(0);
                }

                // You can also instantiate impact effects if needed
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
            // Check if the object hit has a "EnemiesAI" component to apply damage
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.takeDamageFromPlayer(); // Call takeDamageFromPlayer() on the correct target

                // Add score for killing the enemy (100 points)
                if (playerScore != null)
                {
                    playerScore.AddScore(100); // Add 100 points for each enemy killed
                }
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
