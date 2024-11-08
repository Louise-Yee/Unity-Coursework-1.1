using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioController : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioMixer myMixer;

    public void SetMusicVolume(){
        float volume = musicSlider.value;
        myMixer.SetFloat("Master",volume - 80);
    }
}
