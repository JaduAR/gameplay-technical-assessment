using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<AudioClip> _punchClips = new List<AudioClip>();
    [SerializeField] private AudioClip _heavyClip;
    [SerializeField] private AudioClip _missedClip;

    [SerializeField] private float _minPitch = 0.5f;
    [SerializeField] private float _maxPitch = 1.5f;
    [SerializeField] private AudioSource _punchSource;
    [SerializeField] private AudioSource _heavySource;
    [SerializeField] private AudioSource _missedSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void PlayPunchSfx()
    {
        int randomPunchClipIndex = Random.Range(0, _punchClips.Count - 1);
        var clipToPlay = _punchClips[randomPunchClipIndex];
        var pitchValue = Random.Range(_minPitch, _maxPitch);
        _punchSource.pitch = pitchValue;
        _punchSource.PlayOneShot(clipToPlay);
    }

    public void PlayHeavySfx()
    {
        _heavySource.PlayOneShot(_heavyClip);
    }

    public void PlayMissedSfx()
    {
        _missedSource.PlayOneShot(_missedClip);
    }
}
