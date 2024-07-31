using UnityEngine;
using FiveBabbittGames;
using UnityEngine.Audio;

/// <summary>
/// AudioManager
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] public AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayMusic(AudioClip music, bool setLoop = false)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
        musicSource.loop = setLoop;
    }

    public void PlayAmbience(AudioClip ambience, bool setLoop = false)
    {
        ambientSource.Stop();
        ambientSource.clip = ambience;
        ambientSource.Play();
        ambientSource.loop = setLoop;
    }

    public void PlayMusicStinger(AudioClip stinger)
    {
        musicSource.PlayOneShot(stinger);
    }

    public void PlayAmbientStinger(AudioClip stinger)
    {
        ambientSource.PlayOneShot(stinger);
    }
}
