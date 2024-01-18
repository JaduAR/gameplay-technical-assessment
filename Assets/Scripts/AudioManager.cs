using UnityEngine;

//Class with static instance used to control sound effects easily from any script
public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager Instance = null;

    [SerializeField] private AudioClip[] _punchSounds;
    [SerializeField] private AudioClip _cameraSwapSound;
    [SerializeField] private AudioClip _KOSoundEffect;
    [SerializeField] private AudioSource _soundEffectAudSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayPunchAudio()
    {
        _soundEffectAudSource.PlayOneShot(_punchSounds[Random.Range(0, _punchSounds.Length)]);
    }

    public void PlayCameraSwapAudio()
    {
        _soundEffectAudSource.PlayOneShot(_cameraSwapSound);
    }

    public void PlayKOSoundEffect() {
        _soundEffectAudSource.PlayOneShot(_KOSoundEffect);
    }
}
