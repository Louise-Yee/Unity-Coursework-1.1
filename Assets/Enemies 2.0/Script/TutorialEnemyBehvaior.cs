using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyBehavior : MonoBehaviour
{
    // Optional: Reference to play a sound when the enemy dies
    public AudioSource deathSound;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // Method to handle enemy death
    public void Die()
    {
        print("Enemy is dead");

        if (deathSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject tempSoundObject = new GameObject("TempDeathSound");
            AudioSource tempAudioSource = tempSoundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = deathSound.clip;
            tempAudioSource.Play();

            // Destroy the temp object after the sound has finished playing
            Destroy(tempSoundObject, deathSound.clip.length);
        }

        // Destroy the enemy game object
        Destroy(gameObject);
    }
}
