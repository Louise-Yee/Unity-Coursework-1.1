using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBobbing : MonoBehaviour
{
    public PlayerMovement playerMovement; // Reference to PlayerMovement script
    public float bobFrequency = 3.0f; // Speed of the bobbing
    public float bobHorizontalAmplitude = 0.02f; // Reduced side-to-side bobbing amplitude
    public float bobVerticalAmplitude = 0.02f; // Reduced up-and-down bobbing amplitude

    private Vector3 initialPosition;
    private float bobbingTime;
    private float verticalBobOffset;

    void Start()
    {
        // Get the initial local position of the gun
        initialPosition = transform.localPosition;

        // If not manually set, try finding PlayerMovement on the parent or root object
        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError(
                    "PlayerMovement component not found. Please assign it in the Inspector."
                );
            }
        }
    }

    void Update()
    {
        if (playerMovement == null)
            return;

        // Determine if the player is moving or airborne based on velocity
        bool isMoving = playerMovement.GetCurrentSpeed() > 0.1f;
        bool isJumping = !playerMovement.groundedPlayer; // Check if the player is airborne

        if (isMoving)
        {
            // Increment bobbing time based on the frequency
            bobbingTime += Time.deltaTime * bobFrequency;

            // Calculate horizontal bob offset
            float horizontalBob = Mathf.Sin(bobbingTime) * bobHorizontalAmplitude;

            // Smooth vertical bobbing with respect to jumping
            verticalBobOffset = isJumping
                ? Mathf.Lerp(verticalBobOffset, bobVerticalAmplitude, Time.deltaTime * 10f)
                : Mathf.Lerp(verticalBobOffset, 0, Time.deltaTime * 10f);

            // Adjust the bobbing based on movement and jump state
            Vector3 bobbingOffset =
                initialPosition + new Vector3(horizontalBob, verticalBobOffset, 0);
            transform.localPosition = bobbingOffset;
        }
        else
        {
            // Reset to initial position when player stops moving
            bobbingTime = 0;
            verticalBobOffset = Mathf.Lerp(verticalBobOffset, 0, Time.deltaTime * 10f); // Ensure smooth transition
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                initialPosition + new Vector3(0, verticalBobOffset, 0),
                Time.deltaTime * 5f
            );
        }
    }
}
