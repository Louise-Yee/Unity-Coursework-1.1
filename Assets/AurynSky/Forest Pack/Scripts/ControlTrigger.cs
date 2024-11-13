using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTrigger : MonoBehaviour
{
    public enum ControlType
    {
        Move,
        Jump,
        Run,
        Crouch,
        Shoot,
    }

    public ControlType controlToUnlock;
    private TutorialManager tutorialManager;

    public EnemySpawner enemySpawner; // Reference to EnemySpawner

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player") && tutorialManager != null)
    //     {
    //         TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
    //         tutorialManager.ShowNextPanel();
    //         switch (controlToUnlock)
    //         {
    //             case ControlType.Move:
    //                 tutorialManager.UnlockMove();
    //                 break;
    //             case ControlType.Jump:
    //                 tutorialManager.UnlockJump();
    //                 break;
    //             case ControlType.Run:
    //                 tutorialManager.UnlockRun();
    //                 break;
    //             case ControlType.Crouch:
    //                 tutorialManager.UnlockCrouch();
    //                 break;
    //             case ControlType.Shoot:
    //                 tutorialManager.UnlockShoot();
    //                 break;
    //         }

    //         // Call the SpawnFixedNumberOfEnemies function
    //         if (enemySpawner != null)
    //         {
    //             enemySpawner.SpawnFixedNumberOfEnemies();
    //         }
    //         else
    //         {
    //             Debug.LogError("EnemySpawner reference is not assigned.");
    //         }

    //         // Optionally, disable the trigger after activation
    //         gameObject.SetActive(false);
    //     }
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && tutorialManager != null)
        {
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            tutorialManager.ShowNextPanel();

            switch (controlToUnlock)
            {
                case ControlType.Move:
                    tutorialManager.UnlockMove();
                    break;
                case ControlType.Jump:
                    tutorialManager.UnlockJump();
                    break;
                case ControlType.Run:
                    tutorialManager.UnlockRun();
                    break;
                case ControlType.Crouch:
                    tutorialManager.UnlockCrouch();
                    break;
                case ControlType.Shoot:
                    tutorialManager.UnlockShoot();
                    break;
            }

            if (enemySpawner != null)
            {
                enemySpawner.ActivateSpawning();  // Activate spawning here
                enemySpawner.SpawnFixedNumberOfEnemies();  // Initial spawn if needed
            }
            else
            {
                Debug.LogError("EnemySpawner reference is not assigned.");
            }

            gameObject.SetActive(false);  // Optionally disable trigger
        }
    }

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        if (tutorialManager == null)
        {
            Debug.LogError("TutorialManager not found in scene");
        }
    }
}
