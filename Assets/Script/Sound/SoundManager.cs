using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource ambientSource;

    [Header("SFX Clips")]
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip checkpointSound;
    public AudioClip Car;

    [Header("Music/Loop Clips")]
    public AudioClip gameMusic;
    public AudioClip MenuMusic;
    public AudioClip Win;
    public AudioClip Boss;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    [Range(0f, 1f)]
    public float ambientVolume = 0.3f;

    void Awake()
    {
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

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;

        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.loop = true;
        ambientSource.volume = ambientVolume;
    }

    // ========== SFX Methods ==========

    public static void PlaySFX(AudioClip clip)
    {
        if (Instance != null && clip != null)
        {
            Instance.sfxSource.PlayOneShot(clip);
        }
    }

    public static void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (Instance != null && clip != null)
        {
            Instance.sfxSource.PlayOneShot(clip, volumeScale);
        }
    }

    public static void PlaySFXLoop(AudioClip clip)
    {
        if (Instance != null && clip != null)
        {
            Instance.sfxSource.clip = clip;
            Instance.sfxSource.loop = true;
            Instance.sfxSource.Play();
        }
    }

    public static void StopSFXLoop()
    {
        if (Instance != null)
        {
            Instance.sfxSource.loop = false;
            Instance.sfxSource.Stop();
        }
    }

    // ========== Music Methods ==========

    public static void PlayMusic(AudioClip clip)
    {
        if (Instance != null && clip != null)
        {
            Instance.musicSource.clip = clip;
            Instance.musicSource.Play();
        }
    }

    public static void StopMusic()
    {
        if (Instance != null)
        {
            Instance.musicSource.Stop();
        }
    }

    // ========== Ambient Methods ==========

    public static void PlayAmbient(AudioClip clip)
    {
        if (Instance != null && clip != null)
        {
            Instance.ambientSource.clip = clip;
            Instance.ambientSource.Play();
        }
    }

    public static void StopAmbient()
    {
        if (Instance != null)
        {
            Instance.ambientSource.Stop();
        }
    }

    public static void SetAmbientVolume(float volume)
    {
        if (Instance != null)
        {
            Instance.ambientVolume = Mathf.Clamp01(volume);
            Instance.ambientSource.volume = Instance.ambientVolume;
        }
    }

    // ========== Stop All Methods ==========

    public static void StopAllLoops()
    {
        if (Instance != null)
        {
            Instance.musicSource.Stop();
            Instance.ambientSource.Stop();
            Instance.sfxSource.loop = false;
            Instance.sfxSource.Stop();

            Debug.Log("หยุด Loop ทั้งหมดแล้ว");
        }
    }

    public static void StopAllSounds()
    {
        if (Instance != null)
        {
            Instance.musicSource.Stop();
            Instance.ambientSource.Stop();
            Instance.sfxSource.Stop();
            Instance.sfxSource.loop = false;

            Debug.Log("หยุดเสียงทั้งหมดแล้ว");
        }
    }

    // ========== Quick Access Methods - SFX ==========

    public static void PlayJump()
    {
        if (Instance != null && Instance.jumpSound != null)
            PlaySFX(Instance.jumpSound);
    }

    public static void PlayDeath()
    {
        if (Instance != null && Instance.deathSound != null)
            PlaySFX(Instance.deathSound);
    }

    public static void PlayCheckpoint()
    {
        if (Instance != null && Instance.checkpointSound != null)
            PlaySFX(Instance.checkpointSound);
    }

    // ✅ เพิ่ม Quick Access สำหรับเสียงรถ
    public static void PlayCarSound()
    {
        if (Instance != null && Instance.Car != null)
            PlaySFX(Instance.Car);
    }

    public static void PlayCarLoop()
    {
        if (Instance != null && Instance.Car != null)
            PlaySFXLoop(Instance.Car);
    }

    // ========== Quick Access Methods - Music ==========

    // ✅ เพลงเกม
    public static void PlayGameMusic()
    {
        if (Instance != null && Instance.gameMusic != null)
            PlayMusic(Instance.gameMusic);
    }

    // ✅ เพลงเมนู
    public static void PlayMenuMusic()
    {
        if (Instance != null && Instance.MenuMusic != null)
            PlayMusic(Instance.MenuMusic);
    }

    // ✅ เพลงชนะ
    public static void PlayWinMusic()
    {
        if (Instance != null && Instance.Win != null)
            PlayMusic(Instance.Win);
    }

    // ✅ เพลง Boss
    public static void PlayBossMusic()
    {
        if (Instance != null && Instance.Boss != null)
            PlayMusic(Instance.Boss);
    }

    // ========== Volume Control ==========

    public static void SetVolume(float volume)
    {
        if (Instance != null)
        {
            Instance.sfxVolume = Mathf.Clamp01(volume);
            Instance.sfxSource.volume = Instance.sfxVolume;
        }
    }

    public static void SetMusicVolume(float volume)
    {
        if (Instance != null)
        {
            Instance.musicVolume = Mathf.Clamp01(volume);
            Instance.musicSource.volume = Instance.musicVolume;
        }
    }
}