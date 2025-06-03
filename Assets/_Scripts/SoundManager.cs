using UnityEngine;

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

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void PLaySFXSound(AudioClip clip)
    {
        if (clip != null)
        {
            //sfxSource.Stop();
            sfxSource.PlayOneShot(clip);
        }
    }
}
