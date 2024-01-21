using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private List<AudioClip> _punches = null;
    [SerializeField]
    private List<AudioClip> _KOs = null;
    [SerializeField]
    private List<AudioClip> _punchSwooshes = null;

    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void PlayRandomAttackSwoosh()
    {
        if (_punchSwooshes == null || _punchSwooshes.Count == 0) return;

        _audioSource.PlayOneShot(_punchSwooshes[Random.Range(0, _punchSwooshes.Count)]);
    }

    public void PlayRandomPunchSound()
    {
        if (_punches == null || _punches.Count == 0) return;

        _audioSource.PlayOneShot(_punches[Random.Range(0, _punches.Count)]);
    }

    public void PlayRandomKOSound()
    {
        if (_KOs == null || _KOs.Count == 0) return;

        _audioSource.PlayOneShot(_KOs[Random.Range(0, _KOs.Count)]);
    }
}