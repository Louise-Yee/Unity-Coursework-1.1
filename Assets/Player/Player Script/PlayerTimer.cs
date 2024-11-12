using TMPro;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign the GameOver panel here
    public TextMeshProUGUI timerText; // Assign the TextMeshPro text object here for countdown
    public TextMeshProUGUI elapsedTimeText; // Assign the TextMeshPro text object for elapsed time display
    public AudioSource bgmAudioSource; // Reference to the background music AudioSource

    private bool timerActive = false;
    private float timer = 60f; // 1 minute timer
    private float elapsedTime = 0f; // Elapsed time since the game started

    private void Start()
    {
        // Ensure the GameOver panel is initially hidden
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Update the timer displays at the start
        UpdateTimerDisplay();
        UpdateElapsedTimeDisplay();
    }

    private void Update()
    {
        // Only count down if the timer is active
        if (timerActive)
        {
            timer -= Time.deltaTime;
            elapsedTime += Time.deltaTime; // Track the elapsed time
            UpdateTimerDisplay();
            UpdateElapsedTimeDisplay();

            if (timer <= 0)
            {
                EndTimer();
            }
        }
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    private void EndTimer()
    {
        timerActive = false;
        timer = 0f;
        PlayerOptionMenu playerOptionMenu = GetComponent<PlayerOptionMenu>();

        // Show the GameOver panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (bgmAudioSource != null)
        {
            bgmAudioSource.Pause(); // Pauses the music
        }

        if (playerOptionMenu != null)
        {
            playerOptionMenu.isGameOver = true;
        }

        // Pause the game
        Time.timeScale = 0f;

        // Update timer display to show zero
        UpdateTimerDisplay();
        UpdateElapsedTimeDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Display countdown timer in "MM:SS" format
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void UpdateElapsedTimeDisplay()
    {
        if (elapsedTimeText != null)
        {
            // Display elapsed time in "MM:SS" format
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            elapsedTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
}
