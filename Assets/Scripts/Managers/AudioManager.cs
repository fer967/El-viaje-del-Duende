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

    [Header("Efectos de Sonido")]        
    public AudioClip swordAttack;
    public AudioClip wolfmanAttack;
    public AudioClip witchLaugh;
    public AudioClip frogTongue;
    public AudioClip victoryMusic;

    public void PlaySwordAttack() => PlaySFX(swordAttack);
    public void PlayWolfmanAttack() => PlaySFX(wolfmanAttack);
    public void PlayWitchLaugh() => PlaySFX(witchLaugh);
    public void PlayFrogTongue() => PlaySFX(frogTongue);
    
    private AudioSource sfxSource;    
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
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
    private void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "PantallaFinal")
        {
            return;
        }
        string sceneName = scene.name;
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
            PlayMusic(menuMusic, 1.0f);
        }
    }


    private void PlayMusic(AudioClip musicClip, float fadeDuration = 1.0f)
    {
        if (musicClip != null && audioSource != null)
        {
            if (audioSource.clip == musicClip)
            {
                return;
            }
            StartCoroutine(FadeMusic(musicClip, fadeDuration));
        }
    }


    private IEnumerator FadeMusic(AudioClip newClip, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            if (audioSource != null)
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
                yield return null;
            }

        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
            PlayerPrefs.Save();
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
    }


    public void PlayVictoryMusic()
    {
        if (audioSource != null && victoryMusic != null)
        {
            audioSource.Stop();
            audioSource.clip = victoryMusic;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
 
 
}