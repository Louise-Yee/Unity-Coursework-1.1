using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutMenu : MonoBehaviour
{
    public int gameSceneIndex = 1; // Index of the target scene
    public Vector3 teleportPosition = new Vector3(0, 0, 0); // Position to teleport the player to
    public string playerTag = "Player"; // Tag to locate the player

    public void OnWoTutClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(gameSceneIndex);
    }

    public void OnTutClick()
    {
        Debug.Log("Play button clicked. Starting scene load...");
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Start loading the scene asynchronously
        SceneManager.LoadSceneAsync(gameSceneIndex, LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe from the sceneLoaded event to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("Scene loaded successfully!");

        // Find the player in the loaded scene
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            Debug.Log("Player found. Teleporting to " + teleportPosition);
            player.transform.position = teleportPosition;
            Debug.Log("Player teleported to " + teleportPosition);
        }
        else
        {
            Debug.LogError(
                "Player not found! Please check if the player is in the scene and has the correct tag."
            );
        }
    }
}
