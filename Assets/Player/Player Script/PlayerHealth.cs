using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public TextMeshProUGUI healthText; // Reference to the UI Text element
    public GameObject gameOverPanel; // Reference to the Game Over panel
    public AudioSource bgmAudioSource; // Reference to the background music AudioSource
    public GameObject PlayerHUD;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthHUD();

        // Ensure Game Over panel is initially hidden
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        UpdateHealthHUD();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthHUD()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        PlayerHUD.SetActive(false);
        // Get the PlayerOptionMenu component from the same GameObject
        PlayerOptionMenu playerOptionMenu = GetComponent<PlayerOptionMenu>();
        if (playerOptionMenu != null)
        {
            playerOptionMenu.isGameOver = true;
        }

        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Pause(); // Pauses the music
        }

        // Pause the game
        Time.timeScale = 0f;
    }
}
