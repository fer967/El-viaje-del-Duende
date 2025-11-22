using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ConfigMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject configPanel;
    public GameObject settingsWindow;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;


    void Start()
    {
        if (configPanel != null)
            configPanel.SetActive(false);
        if (settingsWindow != null)
            settingsWindow.SetActive(false);
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = 0.7f; 
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            UpdateVolumeText(0.7f);
        }
    }

   
    public void OpenConfig()
    {
        if (configPanel != null)
            configPanel.SetActive(true);
        if (settingsWindow != null)
            settingsWindow.SetActive(true);
        if (AudioManager.instance != null && volumeSlider != null)
        {
            volumeSlider.value = AudioManager.instance.GetMusicVolume();
            UpdateVolumeText(volumeSlider.value);
        }
    }

    
    public void CloseConfig()
    {
        if (configPanel != null)
            configPanel.SetActive(false);
        if (settingsWindow != null)
        {
            settingsWindow.SetActive(false);  
        }
    }


    private void OnVolumeChanged(float volume)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(volume);
        }
        UpdateVolumeText(volume);
    }

    
    private void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            volumeText.text = $"Volumen: {Mathf.RoundToInt(volume * 100)}%";
        }
    }
    
}







