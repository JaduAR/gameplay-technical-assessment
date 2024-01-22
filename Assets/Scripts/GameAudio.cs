using UnityEngine;

public class GameAudio : Singleton<GameAudio> {
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    public AudioClip punch;
    public AudioClip superPunch;
    public AudioClip charge;
    public AudioClip explosion;
    public AudioClip click;

    public AudioClip titleScreen;
    public AudioClip battle;
    public AudioClip win;

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip) {
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }

    public void PlayMusic(AudioClip clip, bool loop) {
        if (musicAudioSource.clip == clip) {
            return;
        }

        musicAudioSource.clip = clip;
        musicAudioSource.loop = loop;
        musicAudioSource.Play();
    }
}
