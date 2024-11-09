using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider; // Reference to the slider
    private const string SensitivityPrefKey = "MouseSensitivity"; // Key to save the sensitivity value

    private void Start()
    {
        // Load saved sensitivity or set default value
        float savedSensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, 1f); // Default sensitivity is 1
        sensitivitySlider.value = savedSensitivity;
    }

    public void OnSensitivityChanged(float newSensitivity)
    {
        PlayerPrefs.SetFloat(SensitivityPrefKey, newSensitivity);
        Debug.Log("Slider value (Sens)L " + newSensitivity);
    }
}
