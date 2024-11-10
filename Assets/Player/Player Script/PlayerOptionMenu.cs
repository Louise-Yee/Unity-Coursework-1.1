using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOptionMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject playerHUD;
    public MonoBehaviour playerInputScript; // Reference to the player input handling script
    public AudioSource bgmAudioSource; // Reference to the background music AudioSource
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game
        playerHUD.SetActive(false); // Hide HUD
        pauseMenu.SetActive(true); // Show pause menu

        // Disable player input script
        if (playerInputScript != null)
        {
            playerInputScript.enabled = false;
        }

        // Stop the background music
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Pause(); // Pauses the music
        }

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game
        playerHUD.SetActive(true); // Show HUD
        pauseMenu.SetActive(false); // Hide pause menu

        // Re-enable player input script
        if (playerInputScript != null)
        {
            playerInputScript.enabled = true;
        }

        // Resume the background music
        if (bgmAudioSource != null)
        {
            bgmAudioSource.UnPause(); // Unpauses the music
        }

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is normal when loading the main menu
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}
