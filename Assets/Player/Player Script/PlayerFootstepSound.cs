using UnityEngine;

public class PlayerFootstepSound : MonoBehaviour
{
    public AudioSource audioSource; // The AudioSource component to play sounds
    public AudioClip grassSound; // Audio clip for grass
    public AudioClip stoneSound; // Audio clip for stone

    private string currentSurface = ""; // Current surface type

    private void Start()
    {
        // Ensure AudioSource is attached
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on this GameObject.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check what layer the player is colliding with
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            currentSurface = "Grass";
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Stone"))
        {
            currentSurface = "Stone";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset current surface when no longer in contact
        if (
            collision.gameObject.layer == LayerMask.NameToLayer("Grass")
            || collision.gameObject.layer == LayerMask.NameToLayer("Stone")
        )
        {
            currentSurface = "";
        }
    }

    public void PlayFootstepSound()
    {
        if (audioSource != null && !string.IsNullOrEmpty(currentSurface))
        {
            switch (currentSurface)
            {
                case "Grass":
                    audioSource.PlayOneShot(grassSound);
                    break;
                case "Stone":
                    audioSource.PlayOneShot(stoneSound);
                    break;
            }
        }
    }
}
