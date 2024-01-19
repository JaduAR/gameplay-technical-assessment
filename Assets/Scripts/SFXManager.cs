using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Simple sound effect manager for the game.
/// </summary>
public partial class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [System.Serializable]
    public class AudioChannelSource
    {
        public AudioChannel Channel;
        public AudioSource Source;
    }

    [Tooltip("Maps between audio channels and audio sources")]
    [SerializeField] 
    private List<AudioChannelSource> _audioChannelSources;

    private Dictionary<AudioChannel, AudioSource> _audioSources;

    [Tooltip("Played when a fighter punches.")]
    [SerializeField]
    private AudioClip _punchClip;

    [Tooltip("Played when a fighter's punch lands on his opponent.")]
    [SerializeField]
    private AudioClip _punchLandingClip;

    [Tooltip("Played when game starts.")]
    [SerializeField]
    private AudioClip _fightClip;

    [Tooltip("Played when game ends.")]
    [SerializeField]
    private AudioClip _koClip;

    [Tooltip("Background music.")]
    [SerializeField]
    private AudioClip _backgroundMusicClip;

    private void Awake()
    {
        Instance = this;
        InitializeAudioSources();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Subscribe to game events to play their sounds.
    /// </summary>
    private void SubscribeToEvents()
    {
        foreach(var fighter in GameManager.Instance.Fighters)
        {
            fighter.OnPunch += PlayPunch;
            fighter.OnPunchLanded += PlayPunchLanded;
        }
        GameManager.Instance.OnGameStart += PlayFight;
        GameManager.Instance.OnGameEnd += PlayKO;
    }

    /// <summary>
    /// Unsubscribe from game events.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        foreach (var fighter in GameManager.Instance.Fighters)
        {
            fighter.OnPunch -= PlayPunch;
            fighter.OnPunchLanded -= PlayPunchLanded;
        }
        GameManager.Instance.OnGameStart -= PlayFight;
        GameManager.Instance.OnGameEnd -= PlayKO;
    }

    private void InitializeAudioSources()
    {
        _audioSources = new Dictionary<AudioChannel, AudioSource>();
        foreach (var channelSource in _audioChannelSources)
            _audioSources[channelSource.Channel] = channelSource.Source;
        
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Play a sound effect.
    /// </summary>
    /// <param name="clip">Clip to be played.</param>
    /// <param name="channel">Channel on which the provided audio clip will be played.</param>
    public void PlaySFX(AudioClip clip, AudioChannel channel = AudioChannel.SFX)
    {
        if (_audioSources.TryGetValue(channel, out var source))
            source.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic()
    {
        if (!_audioSources.TryGetValue(AudioChannel.Music, out var musicSource)) return;
        musicSource.clip = _backgroundMusicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPunch() => PlaySFX(_punchClip);
    public void PlayPunchLanded() => PlaySFX(_punchLandingClip);
    public void PlayFight() => PlaySFX(_fightClip, AudioChannel.Voice);
    public void PlayKO() => PlaySFX(_koClip, AudioChannel.Voice);

}