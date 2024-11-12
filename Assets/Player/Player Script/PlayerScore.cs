using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int currentScore = 0; // The player's score
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro UI Text that will display the score
    public TextMeshProUGUI finalScoreText; // Reference to the TextMeshPro UI Text that will display the score

    void Start()
    {
        UpdateScoreUI(); // Update the score display at the start of the game
    }

    // Call this method to add points to the score
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI(); // Update the score UI
    }

    // Update the score UI
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString(); // Display score
        }
        if (finalScoreText != null)
        {
            finalScoreText.text = currentScore.ToString();
        }
    }

    // Optionally, call this when the player dies or the game ends
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI(); // Update UI after reset
    }
}
