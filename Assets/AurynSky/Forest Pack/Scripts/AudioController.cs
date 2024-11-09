using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioController : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer myMixer;

    private const float MinDecibels = -80f; // Define the minimum dB level for silence

    private void Start()
    {
        // Load saved volume preference, or default to max volume (100)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 100);
        musicSlider.value = savedVolume;
        SetMusicVolume(); // Apply initial volume based on saved value
    }

    public void SetMusicVolume()
    {
        // Convert the slider's value (0 to 100) to a 0-1 range
        float normalizedVolume = musicSlider.value / 100f;

        float dBValue;
        if (normalizedVolume <= 0.01f) // Near-zero values should result in silence
        {
            dBValue = MinDecibels;
        }
        else
        {
            dBValue = 20 * Mathf.Log10(normalizedVolume) + 10; // Convert to dB scale
        }

        // Apply the calculated dB value to the Audio Mixer
        myMixer.SetFloat("Master", dBValue);

        // Save the slider value for future sessions
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }
}

