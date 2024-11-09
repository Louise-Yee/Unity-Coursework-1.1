using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isMoveUnlocked = false;
    public bool isJumpUnlocked = false;
    public bool isCrouchUnlocked = false;
    public bool isRunUnlocked = false;
    public bool isShootUnlocked = false;

    public GameObject[] tutorialPanels;
    private int currentPanelIndex = 0;

    public void UnlockMove() => isMoveUnlocked = true;
    public void UnlockJump() => isJumpUnlocked = true;
    public void UnlockCrouch() => isCrouchUnlocked = true;
    public void UnlockRun() => isRunUnlocked = true;
    public void UnlockShoot() => isShootUnlocked = true;

    public void ShowNextPanel(){
        if(currentPanelIndex < tutorialPanels.Length - 1){
            tutorialPanels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            tutorialPanels[currentPanelIndex].SetActive(true);
        }
    }
}

