using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to disable (the one containing this script)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }

            // Start the timer on the player
            PlayerTimer playerTimer = other.GetComponent<PlayerTimer>();
            if (playerTimer != null)
            {
                playerTimer.StartTimer();
            }
        }
    }
}
