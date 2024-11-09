using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockControls : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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

