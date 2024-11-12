using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScore : MonoBehaviour
{
    // Reference to the PlayerScore component
    private PlayerScore playerScore;

    // Start is called before the first frame update
    void Start()
    {
        // Find the PlayerScore component on the player GameObject (assuming the player has the tag "Player")
        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();
    }

    // This method will be called when something enters the trigger collider of this GameObject
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerScore != null)
            {
                playerScore.ResetScore();
            }
            else
            {
                print("PlayerScore is null");
            }
        }
    }
}
