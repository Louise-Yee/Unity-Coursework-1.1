using UnityEngine;

public class PlayerFootstepSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] grassSounds; // Array of grass audio clips
    public AudioClip stoneSound;
    public AudioClip jumpSound; // Audio clip for jump

    [Header("Surface Detection")]
    public Transform groundCheckObject;
    public float groundCheckDistance = 0.5f;
    public LayerMask surfaceLayers;

    [Header("Footstep Settings")]
    public float baseStepInterval = 0.5f;
    public float speedMultiplier = 1.5f;

    private string currentSurface = "";
    private PlayerMovement playerMovement;
    private float footstepTimer = 0f;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on this GameObject.");
            }
        }

        playerMovement = GetComponentInParent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on this GameObject.");
        }

        if (groundCheckObject == null)
        {
            Debug.LogError("GroundCheckObject is not assigned.");
        }
    }

    private void Update()
    {
        DetectSurface();

        if (playerMovement != null && playerMovement.groundedPlayer && playerMovement.GetCurrentSpeed() > 0.1f)
        {
            footstepTimer -= Time.deltaTime;

            float dynamicStepInterval = baseStepInterval / (playerMovement.GetCurrentSpeed() / 6f);
            dynamicStepInterval = Mathf.Clamp(dynamicStepInterval, 0.2f, 1f);

            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = dynamicStepInterval;
            }
        }
    }

    private void DetectSurface()
    {
        if (groundCheckObject == null) return;

        RaycastHit hit;
        if (Physics.Raycast(groundCheckObject.position, Vector3.down, out hit, groundCheckDistance, surfaceLayers))
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Grass"))
            {
                currentSurface = "Grass";
            }
            else if (layer == LayerMask.NameToLayer("Stone"))
            {
                currentSurface = "Stone";
            }
            else
            {
                currentSurface = "";
            }
        }
        else
        {
            currentSurface = "";
        }
    }

    public void PlayFootstepSound()
    {
        if (audioSource != null && !string.IsNullOrEmpty(currentSurface))
        {
            float playerSpeed = playerMovement.GetCurrentSpeed();
            float pitchMultiplier = Mathf.Clamp(playerSpeed / 15f, 0.8f, 1.5f);
            audioSource.pitch = pitchMultiplier;

            if (currentSurface == "Grass" && grassSounds.Length > 0)
            {
                AudioClip randomGrassSound = grassSounds[Random.Range(0, grassSounds.Length)];
                audioSource.PlayOneShot(randomGrassSound);
            }
            else if (currentSurface == "Stone")
            {
                audioSource.PlayOneShot(stoneSound);
            }
        }
    }

    public void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
            Debug.Log("Playing jump sound.");
        }
    }
}
