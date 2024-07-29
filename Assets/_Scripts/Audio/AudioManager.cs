using UnityEngine;
using FiveBabbittGames;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// AudioManager
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    
    protected override void Awake()
    {
        base.Awake();

    }

    public void PlayMusic(AudioClip music, bool setLoop = false)
    {
        musicSource.PlayOneShot(music);
        musicSource.loop = setLoop;
    }

    public void PlayAmbience(AudioClip ambience, bool setLoop = false)
    {
        ambientSource.PlayOneShot(ambience);
        ambientSource.loop = setLoop;
    }
}
