using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    [Header("SOURCES")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("SOUNDS")]
    [SerializeField] public AudioClip playerTakeDamage;
    [SerializeField] public AudioClip broTakeDamage;
    [SerializeField] public AudioClip hoverCard;
    [SerializeField] public AudioClip shuffle;
    [SerializeField] public AudioClip endTurn;
    [SerializeField] public AudioClip changeMask;
    [SerializeField] private AudioClip button;

    [Header("MUSICS")]
    public AudioClip backgroundMusic;
    public AudioClip mainMenuMusic;

    private void Start()
    {
        SetButtonSound();
        PlayMusic(mainMenuMusic);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlayMusic(AudioClip music)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
        sfxSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
        sfxSource.UnPause();
    }

    public void PLaySFXSound(AudioClip clip)
    {
        if (clip != null)
        {
            //sfxSource.Stop();
            sfxSource.PlayOneShot(clip);
        }
    }

    private void SetButtonSound()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PLaySFXSound(button));
        }
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }
}
