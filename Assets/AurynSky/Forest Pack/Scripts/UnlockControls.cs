using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockControls : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to EnemySpawner
    private void OnTriggerEnter(Collider other)
    {
        enemySpawner.ActivateSpawning();  // Activate spawning here
        if (other.CompareTag("Player"))
        {
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            tutorialManager.UnlockMove();
            tutorialManager.UnlockJump();
            tutorialManager.UnlockRun();
            tutorialManager.UnlockCrouch();
            tutorialManager.UnlockShoot();
        }

        // Optionally, disable the trigger after activation
        gameObject.SetActive(false);
    }
}

