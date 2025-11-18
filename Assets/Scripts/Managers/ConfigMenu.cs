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
        // DESACTIVAR COMPLETAMENTE al inicio
        if (configPanel != null)
            configPanel.SetActive(false);

        if (settingsWindow != null)
            settingsWindow.SetActive(false);

        // Configurar slider básico (sin audio manager todavía)
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = 0.7f; // Valor por defecto
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            UpdateVolumeText(0.7f);
        }
    }

    //void Start()
    //{
    //    // Configurar UI
    //    if (configPanel != null)
    //        configPanel.SetActive(false);   // habia puesto true

    //    if (settingsWindow != null)
    //        settingsWindow.SetActive(false);

    //    // Configurar slider
    //    if (volumeSlider != null)
    //    {
    //        volumeSlider.minValue = 0f;
    //        volumeSlider.maxValue = 1f;
    //        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

    //        // Inicializar con valor por defecto, luego actualizar
    //        volumeSlider.value = 0.7f;
    //        UpdateVolumeText(0.7f);

    //        // Corutina para actualizar cuando AudioManager esté listo
    //        StartCoroutine(WaitForAudioManager());
    //    }
    //}

    //private IEnumerator WaitForAudioManager()
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    if (AudioManager.instance != null)
    //    {
    //        volumeSlider.value = AudioManager.instance.GetMusicVolume();
    //        UpdateVolumeText(volumeSlider.value);
    //    }
    //}


    public void OpenConfig()
    {
        if (configPanel != null)
            configPanel.SetActive(true);

        if (settingsWindow != null)
            settingsWindow.SetActive(true);

        // INICIALIZAR AUDIO SETTINGS AQUÍ (cuando se abre el menú)
        if (AudioManager.instance != null && volumeSlider != null)
        {
            volumeSlider.value = AudioManager.instance.GetMusicVolume();
            UpdateVolumeText(volumeSlider.value);
        }
    }

    //public void OpenConfig()
    //{
    //    if (configPanel != null)
    //        configPanel.SetActive(true);

    //    if (settingsWindow != null)
    //        settingsWindow.SetActive(true);

    //    // Siempre verificar que AudioManager existe
    //    if (AudioManager.instance != null)
    //    {
    //        volumeSlider.value = AudioManager.instance.GetMusicVolume();
    //        UpdateVolumeText(volumeSlider.value);
    //    }
    //}

    //public void OpenConfig()
    //{
    //    if (configPanel != null)
    //    {
    //        configPanel.SetActive(true);
    //        // Cargar valores actuales
    //        volumeSlider.value = AudioManager.instance.GetMusicVolume();
    //        UpdateVolumeText(volumeSlider.value);
    //    }

    //    // AGREGAR ESTAS LÍNEAS:
    //    if (settingsWindow != null)
    //    {
    //        settingsWindow.SetActive(true);  // Activar la ventana visible
    //    }
    //}

    public void CloseConfig()
    {
        if (configPanel != null)
            configPanel.SetActive(false);
        // AGREGAR ESTAS LÍNEAS:
        if (settingsWindow != null)
        {
            settingsWindow.SetActive(false);  // Desactivar la ventana visible
        }
    }


    private void OnVolumeChanged(float volume)
    {
        // VERIFICAR SI AUDIOMANAGER EXISTE ANTES DE USARLO
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(volume);
        }
        else
        {
            Debug.LogWarning("AudioManager no está disponible");
        }

        UpdateVolumeText(volume);
    }

    //private void OnVolumeChanged(float volume)
    //{
    //    AudioManager.instance.SetMusicVolume(volume);
    //    UpdateVolumeText(volume);
    //}


    private void UpdateVolumeText(float volume)
    {
        if (volumeText != null)
        {
            volumeText.text = $"Volumen: {Mathf.RoundToInt(volume * 100)}%";
        }
    }

    
}







//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class ConfigMenu : MonoBehaviour
//{
//    [Header("UI Elements")]
//    public GameObject configPanel;
//    public GameObject settingsWindow;   // agrego
//    public Slider volumeSlider;
//    public TextMeshProUGUI volumeText;

//    void Start()
//    {
//        // Configurar slider de volumen
//        if (volumeSlider != null)
//        {
//            volumeSlider.minValue = 0f;
//            volumeSlider.maxValue = 1f;
//            volumeSlider.value = AudioManager.instance.GetMusicVolume();
//            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

//            UpdateVolumeText(volumeSlider.value);
//        }

//        // Ocultar panel al inicio
//        if (configPanel != null)
//            configPanel.SetActive(true);

//        // SettingsWindow INACTIVO al inicio
//        if (settingsWindow != null)
//            settingsWindow.SetActive(false);
//    }



//public void OpenConfig()
//{
//    if (configPanel != null)
//    {
//        configPanel.SetActive(true);
//        // Cargar valores actuales
//        volumeSlider.value = AudioManager.instance.GetMusicVolume();
//        UpdateVolumeText(volumeSlider.value);
//    }

//    // AGREGAR ESTAS LÍNEAS:
//    if (settingsWindow != null)
//    {
//        settingsWindow.SetActive(true);  // Activar la ventana visible
//    }
//}

//public void OpenConfig()
//{
//    if (configPanel != null)
//    {
//        configPanel.SetActive(true);
//        // Cargar valores actuales
//        volumeSlider.value = AudioManager.instance.GetMusicVolume();
//        UpdateVolumeText(volumeSlider.value);
//    }

//    // AGREGAR ESTAS LÍNEAS:
//    if (settingsWindow != null)
//    {
//        settingsWindow.SetActive(true);  // Activar la ventana visible
//    }
//}

//public void CloseConfig()
//{
//    if (configPanel != null)
//        configPanel.SetActive(false);
//    // AGREGAR ESTAS LÍNEAS:
//    if (settingsWindow != null)
//    {
//        settingsWindow.SetActive(false);  // Desactivar la ventana visible
//    }
//}

//private void OnVolumeChanged(float volume)
//{
//    AudioManager.instance.SetMusicVolume(volume);
//    UpdateVolumeText(volume);
//}

//private void UpdateVolumeText(float volume)
//{
//    if (volumeText != null)
//        volumeText.text = $"Volumen: {Mathf.RoundToInt(volume * 100)}%";
//}


//}
