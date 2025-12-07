using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource; // Background Music
    public AudioSource sfxSource; // Sound Effects

    [Header("Background Music Clips")]
    public AudioClip bgmHome;
    public AudioClip bgmCharacterSelection;
    public AudioClip bgmMapSelection;
    public AudioClip bgmBattle;
    public AudioClip bgmResult;

    [Header("Sound Effects")]
    public AudioClip sfxButtonClick;
    public AudioClip sfxCharacterSelect;
    public AudioClip sfxAttack;
    public AudioClip sfxHurt;
    public AudioClip sfxDeath;
    public AudioClip sfxVictory;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAudioSources()
    {
        // Tạo AudioSource cho BGM nếu chưa có
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = bgmVolume;

        // Tạo AudioSource cho SFX nếu chưa có
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    // ==================== BACKGROUND MUSIC ====================

    public void PlayBGM(AudioClip clip, bool forceRestart = false)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null!");
            return;
        }

        // Nếu đang phát cùng bài nhạc thì không cần restart
        if (bgmSource.clip == clip && bgmSource.isPlaying && !forceRestart)
            return;

        bgmSource.clip = clip;
        bgmSource.Play();
        Debug.Log("Playing BGM: " + clip.name);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }

    // ==================== SCENE-SPECIFIC BGM ====================

    public void PlayHomeBGM()
    {
        PlayBGM(bgmHome);
    }

    public void PlayCharacterSelectionBGM()
    {
        PlayBGM(bgmCharacterSelection);
    }

    public void PlayMapSelectionBGM()
    {
        PlayBGM(bgmMapSelection);
    }

    public void PlayBattleBGM()
    {
        PlayBGM(bgmBattle);
    }

    public void PlayResultBGM()
    {
        PlayBGM(bgmResult);
    }

    // ==================== SOUND EFFECTS ====================

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX AudioClip is null!");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayButtonClick()
    {
        PlaySFX(sfxButtonClick);
    }

    public void PlayCharacterSelect()
    {
        PlaySFX(sfxCharacterSelect);
    }

    public void PlayAttackSound()
    {
        PlaySFX(sfxAttack);
    }

    public void PlayHurtSound()
    {
        PlaySFX(sfxHurt);
    }

    public void PlayDeathSound()
    {
        PlaySFX(sfxDeath);
    }

    public void PlayVictorySound()
    {
        PlaySFX(sfxVictory);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    // ==================== FADE EFFECTS ====================

    public void FadeOutBGM(float duration = 1f)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    public void FadeInBGM(float duration = 1f)
    {
        StartCoroutine(FadeInCoroutine(duration));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = bgmSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }

    private System.Collections.IEnumerator FadeInCoroutine(float duration)
    {
        float targetVolume = bgmVolume;
        bgmSource.volume = 0f;

        if (!bgmSource.isPlaying)
            bgmSource.Play();

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }
}
