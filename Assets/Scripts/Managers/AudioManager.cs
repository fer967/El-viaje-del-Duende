using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Música")]
    public AudioClip menuMusic;
    public AudioClip forestMusic;
    public AudioClip caveMusic;

    private AudioSource audioSource;
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const float DEFAULT_VOLUME = 0.7f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            LoadVolumeSettings();

            // SUSCRIBIRSE AL EVENTO DE CAMBIO DE ESCENA
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // DESUSCRIBIRSE AL DESTRUIRSE
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // ESTE MÉTODO SE LLAMA AUTOMÁTICAMENTE CUANDO CAMBIA LA ESCENA

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log($"=== CAMBIO DE ESCENA ===");
    //    Debug.Log($"Escena: {scene.name}");

    //    string sceneName = scene.name.ToLower();

    //    if (IsMenuScene(sceneName))
    //    {
    //        Debug.Log("🎵 Solicitando música de MENÚ");
    //        PlayMusic(menuMusic, 1.0f);
    //    }
    //    else if (IsForestScene(sceneName))
    //    {
    //        Debug.Log("🌲 Solicitando música de BOSQUE");
    //        PlayMusic(forestMusic, 1.5f);  // ← ¡DEBE SER forestMusic, NO menuMusic!
    //    }
    //    else if (IsCaveScene(sceneName))
    //    {
    //        Debug.Log("🏔️ Solicitando música de CUEVA");
    //        PlayMusic(caveMusic, 1.5f);    // ← ¡DEBE SER caveMusic, NO menuMusic!
    //    }
    //    else
    //    {
    //        Debug.Log("🎵 Solicitando música de MENÚ (por defecto)");
    //        PlayMusic(menuMusic, 1.0f);
    //    }
    //}


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Escena cargada: {scene.name}");

        //string sceneName = scene.name.ToLower();
        string sceneName = scene.name;

        // AJUSTÁ ESTOS NOMBRES SEGÚN TUS ESCENAS REALES
        if (sceneName.Contains("SampleScene") || sceneName.Contains("MenuInicio"))
        {
            PlayMusic(menuMusic, 1.0f);
        }
        else if (sceneName.Contains("Bosque1") || sceneName.Contains("Bosque2") ||
                 sceneName.Contains("Bosque3") || sceneName.Contains("Bosque4") ||
                 sceneName.Contains("Bosque5") || sceneName.Contains("Bosque6"))
        {
            PlayMusic(forestMusic, 1.5f);
        }
        else if (sceneName.Contains("Cueva1") || sceneName.Contains("Cueva2") ||
                 sceneName.Contains("Cueva3"))
        {
            PlayMusic(caveMusic, 1.5f);
        }
        else
        {
            // Por defecto
            PlayMusic(menuMusic, 1.0f);
        }
    }


    private void PlayMusic(AudioClip musicClip, float fadeDuration = 1.0f)
    {
        Debug.Log($"=== PLAY MUSIC CALLED ===");
        Debug.Log($"Clip solicitado: {musicClip?.name}");
        Debug.Log($"Clip actual: {audioSource?.clip?.name}");

        if (musicClip != null && audioSource != null)
        {
            // SOLO evitar cambio si es EXACTAMENTE el mismo AudioClip
            if (audioSource.clip == musicClip)
            {
                Debug.Log("🚫 Ya está reproduciendo este clip, no hacer cambio");
                return;
            }

            // SIEMPRE cambiar si son clips diferentes (aunque ambos sean música de menú)
            Debug.Log($"🔄 Cambiando música de {audioSource.clip?.name} a: {musicClip.name}");
            StartCoroutine(FadeMusic(musicClip, fadeDuration));
        }
        else
        {
            Debug.LogError($"❌ Error: musicClip null: {musicClip == null}, audioSource null: {audioSource == null}");
        }
    }


    // modificacion
    //private void PlayMusic(AudioClip musicClip, float fadeDuration = 1.0f)
    //{
    //    Debug.Log($"=== PLAY MUSIC CALLED ===");
    //    Debug.Log($"Clip solicitado: {musicClip?.name}");
    //    Debug.Log($"Clip actual: {audioSource?.clip?.name}");
    //    Debug.Log($"AudioSource isPlaying: {audioSource?.isPlaying}");
    //    Debug.Log($"Fade duration: {fadeDuration}");

    //    if (musicClip != null && audioSource != null)
    //    {
    //        // Si ya está reproduciendo este clip, no hacer nada
    //        if (audioSource.clip == musicClip && audioSource.isPlaying)
    //        {
    //            Debug.Log("🚫 Ya está reproduciendo este clip, no hacer cambio");
    //            return;
    //        }

    //        Debug.Log($"🔄 Cambiando música a: {musicClip.name}");
    //        StartCoroutine(FadeMusic(musicClip, fadeDuration));
    //    }
    //    else
    //    {
    //        Debug.LogError($"❌ Error: musicClip null: {musicClip == null}, audioSource null: {audioSource == null}");
    //    }
    //}

    private IEnumerator FadeMusic(AudioClip newClip, float fadeDuration)
    {
        Debug.Log($"🎵 Iniciando fade a: {newClip.name}");

        // Fade out
        float startVolume = audioSource.volume;
        Debug.Log($"Fade out desde volumen: {startVolume}");

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            if (audioSource != null)
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        if (audioSource != null)
        {
            Debug.Log($"🔄 Cambiando clip a: {newClip.name}");
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();

            Debug.Log($"Nuevo clip asignado: {audioSource.clip?.name}");
            Debug.Log($"AudioSource isPlaying: {audioSource.isPlaying}");

            // Fade in
            Debug.Log($"Fade in hasta volumen: {startVolume}");
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
                yield return null;
            }

            Debug.Log($"✅ Fade completado. Clip actual: {audioSource.clip?.name}");
        }
    }


    //private void PlayMusic(AudioClip musicClip, float fadeDuration = 1.0f)
    //{
    //    if (musicClip != null && audioSource != null)
    //    {
    //        // Si ya está reproduciendo este clip, no hacer nada
    //        if (audioSource.clip == musicClip && audioSource.isPlaying)
    //            return;

    //        StartCoroutine(FadeMusic(musicClip, fadeDuration));
    //    }
    //}


    //private IEnumerator FadeMusic(AudioClip newClip, float fadeDuration)
    //{
    //    // Fade out
    //    float startVolume = audioSource.volume;

    //    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    //    {
    //        if (audioSource != null)
    //            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
    //        yield return null;
    //    }

    //    if (audioSource != null)
    //    {
    //        audioSource.Stop();
    //        audioSource.clip = newClip;
    //        audioSource.Play();

    //        // Fade in
    //        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    //        {
    //            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
    //            yield return null;
    //        }
    //    }
    //}


    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
            PlayerPrefs.Save();
            Debug.Log($"Volumen cambiado a: {volume}");
        }
        else
        {
            Debug.LogError("AudioSource es null en AudioManager");
        }
    }

    public float GetMusicVolume()
    {
        if (audioSource != null)
        {
            return audioSource.volume;
        }
        return DEFAULT_VOLUME;
    }

    private void LoadVolumeSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        if (audioSource != null)
        {
            audioSource.volume = savedVolume;
        }
        Debug.Log($"Volumen cargado: {savedVolume}");
    }

    
}