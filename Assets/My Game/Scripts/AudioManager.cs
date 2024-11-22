using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[AddComponentMenu("TrannChanhh/AudioManager")]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance;

    [Header("Audio Clips background")]
    public List<AudioClip> backgroundMusicTracks;
    public AudioClip detectionMusic;
    public AudioClip backgroundMusic;
    public AudioClip clickAudio;
    public AudioClip GamePlayMusic;

    [Header("Audio Clips Enemy")]
    public AudioClip enemyFootsteps;
    public AudioClip enemyNoise;
    public AudioClip enemyDead;
    public AudioClip enemyAttack;

    private int currentTrackIndex = 0;
    private bool isDetected = false;
    private AudioClip originalMusic;

    [Header("Audio Clips Player")]
    public AudioClip Bows; // sound when shooting randomly
    public AudioClip ArrowSound; // sound when hit
    public AudioClip Sword; // sound of slashing
    public AudioClip SwordSound; // sound when hit
    public AudioClip playerAttacked; // get hit by enemy
    public AudioClip playerWalk;
    public AudioClip playerRun;
    public AudioClip playerJump;
    public AudioClip playerDead;

    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource detectionMusicSource; // Additional source for detection music
    public AudioSource sfxUISource;
    public AudioSource enemySource;
    public AudioSource playerSource;

    public float fadeDuration = 1f; // Duration for crossfade

    [Header("ControlAutio")]
    [Range(0, 1)] public float backgroundVolume = 1.0f;
    [Range(0, 1)] public float sfxVolume = 1.0f;

    public Slider backgroundVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Configure 3D audio settings for enemySource with Logarithmic rolloff
        enemySource.spatialBlend = 2f;             // Set sound to 3D
        enemySource.rolloffMode = AudioRolloffMode.Logarithmic; // Logarithmic volume attenuation
        enemySource.minDistance = 1f;                // Full volume within 2 units
        enemySource.maxDistance = 2f;               // Volume fades out at 20 units
    }

    private void OnEnable()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
        PlayNextTrack();
        if (backgroundVolumeSlider != null)
        {
            backgroundVolumeSlider.value = backgroundVolume;
            backgroundVolumeSlider.onValueChanged.AddListener(SetBackgroundVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // Method to set background music volume
    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = volume;
        musicSource.volume = backgroundVolume;
    }

    // Method to set SFX volume
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxUISource.volume = sfxVolume;
        enemySource.volume = sfxVolume;
        playerSource.volume = sfxVolume;
    }


    private void Update()
    {
        // If not detected, play the next track in sequence if music has ended
        if (!isDetected && !musicSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    // Play next music

    public void PlayNextTrack()
    {
        if (backgroundMusicTracks.Count == 0) return;

        // Set the next track and start playing it
        musicSource.clip = backgroundMusicTracks[currentTrackIndex];
        musicSource.loop = false;  // Set to false for track sequencing
        musicSource.Play();

        // Move to the next track in the list, looping back if at the end
        currentTrackIndex = (currentTrackIndex + 1) % backgroundMusicTracks.Count;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop current music and play new music for the new scene
        StopMusic();
        PlayMusicForScene(scene.name);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySfxUI(AudioClip clip)
    {
        sfxUISource.PlayOneShot(clip);
    }

    public void PlayOnClick(float speed = 1.5f, float volume = 1.0f)
    {
        sfxUISource.pitch = speed;
        sfxUISource.volume = Mathf.Clamp(volume, 0f, 1f);
        PlaySfxUI(clickAudio);
        sfxUISource.pitch = 1.0f;
        sfxUISource.volume = 1.0f;
    }

    // Change combat status Music

    public void StartDetectionMusic()
    {
        if (isDetected) return; // Prevent restarting if already detected

        isDetected = true;
        originalMusic = musicSource.clip; // Save the current track to resume later
        StartCoroutine(CrossfadeMusic(musicSource, detectionMusicSource, detectionMusic, fadeDuration));
    }

    public void StopDetectionMusic()
    {
        if (!isDetected) return; // Only resume if previously detected

        isDetected = false;
        StartCoroutine(CrossfadeMusic(detectionMusicSource, musicSource, originalMusic, fadeDuration));
    }

    private IEnumerator CrossfadeMusic(AudioSource fromSource, AudioSource toSource, AudioClip toClip, float duration)
    {
        float startVolumeFrom = fromSource.volume;
        float startVolumeTo = toSource.volume;
        float time = 0;

        // Set the clip for the "to" source and start playing it if not already playing
        toSource.clip = toClip;
        if (!toSource.isPlaying)
            toSource.Play();

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Fade out the "from" source and fade in the "to" source
            fromSource.volume = Mathf.Lerp(startVolumeFrom, 0, t);
            toSource.volume = Mathf.Lerp(startVolumeTo, 1, t);

            yield return null;
        }

        // Ensure final volumes are set
        fromSource.volume = 0;
        toSource.volume = 0.5f;

        // Stop the "from" source to save resources
        fromSource.Stop();
    }

    // Change scene Music

    private void PlayMusicForScene(string sceneName)
    {
        // Choose background music based on the scene name
        AudioClip newMusic = null;

        if (sceneName == "Main_Menu")
        {
            newMusic = backgroundMusic;
        }
        else if (sceneName == "Loading Scene")
        {
            newMusic = null;
        }
        else if (sceneName == "Main")
        {
            newMusic = GamePlayMusic;
        }

        // Add more scenes as needed

        if (newMusic != null)
        {
            PlayMusic(newMusic);
        }
    }
}
