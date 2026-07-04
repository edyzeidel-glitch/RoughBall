using UnityEngine;

// Central sound player. All clips are generated procedurally (self-made, license-safe).
public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    [Header("Clips")]
    public AudioClip hit;
    public AudioClip pickup;
    public AudioClip shield;
    public AudioClip levelUp;
    public AudioClip gameOver;
    public AudioClip victory;
    public AudioClip music;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    void Awake()
    {
        I = this;
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.45f;
    }

    void Start()
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    void Play(AudioClip c, float vol)
    {
        if (c != null) sfxSource.PlayOneShot(c, vol);
    }

    public void PlayHit() { Play(hit, 0.9f); }
    public void PlayPickup() { Play(pickup, 0.8f); }
    public void PlayShield() { Play(shield, 0.8f); }
    public void PlayLevelUp() { Play(levelUp, 0.8f); }
    public void PlayGameOver() { musicSource.Stop(); Play(gameOver, 1f); }
    public void PlayVictory() { musicSource.Stop(); Play(victory, 1f); }
}
