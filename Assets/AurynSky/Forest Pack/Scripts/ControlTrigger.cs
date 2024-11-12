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

            // Optionally, disable the trigger after activation
            gameObject.SetActive(false);
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
