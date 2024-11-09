using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource buttonSource;
    
    [Header("---------- Audio Clip ----------")]
    public AudioClip bgm;
    public AudioClip buttonClickSound;

    private void Awake()
    {
        // Check if there's an existing instance of AudioManager
        if (Instance == null)
        {
            Instance = this; // Set this instance as the Singleton
        }
        else
        {
            Destroy(gameObject); // Destroy any extra instances
        }
    }
    private void Start(){
        musicSource.clip = bgm;
        musicSource.Play();
    }

    public void playButtonClickSound(){
        if(buttonSource != null && buttonClickSound != null){
            buttonSource.PlayOneShot(buttonClickSound);
        }
    }
}