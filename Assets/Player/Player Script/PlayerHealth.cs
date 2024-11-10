using UnityEngine;
using UnityEngine.UI; // Import UI namespace

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Max health
    public int currentHealth; // Current health

    public Text healthText; // Reference to the UI Text element

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthHUD();
    }

    // Function to take damage
    public void TakeDamage()
    {
        currentHealth--;
        UpdateHealthHUD(); // Update the HUD each time health changes

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void UpdateHealthHUD()
    {
        // Display the current health on the HUD
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        // Logic to change the scene to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
