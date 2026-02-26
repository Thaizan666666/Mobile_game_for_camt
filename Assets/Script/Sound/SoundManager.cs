using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("SFX Clips")]
    public AudioClip jumpSound;
    public AudioClip deathSound;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // สร้าง AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = sfxVolume;
    }

    // ========== Methods ==========

    public static void PlaySFX(AudioClip clip)
    {
        if (Instance != null && clip != null)
        {
            Instance.audioSource.PlayOneShot(clip);
        }
    }

    public static void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (Instance != null && clip != null)
        {
            Instance.audioSource.PlayOneShot(clip, volumeScale);
        }
    }

    // ========== Quick Access ==========

    public static void PlayJump()
    {
        if (Instance != null && Instance.jumpSound != null)
        {
            PlaySFX(Instance.jumpSound);
        }
    }

    public static void PlayDeath()
    {
        if (Instance != null && Instance.deathSound != null)
        {
            PlaySFX(Instance.deathSound);
        }
    }

    public static void SetVolume(float volume)
    {
        if (Instance != null)
        {
            Instance.sfxVolume = Mathf.Clamp01(volume);
            Instance.audioSource.volume = Instance.sfxVolume;
        }
    }
}