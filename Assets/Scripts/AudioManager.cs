using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    [Header("Audio Clips")]
    public List<Sound> musicSounds;  // Music sounds list
    public List<Sound> sfxSounds;    // SFX sounds list

    private AudioSource taskAudioSource; // Added audio source for task-related audio

    protected override void Awake()
    {
        base.Awake();  // Calls the base class Awake method for singleton initialization

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        InitializeSounds(musicSounds, musicVolume);
        InitializeSounds(sfxSounds, sfxVolume);

        // Add dedicated audio source for task-related audio
        taskAudioSource = gameObject.AddComponent<AudioSource>();
        taskAudioSource.volume = sfxVolume;
        taskAudioSource.loop = false; // Task audio doesn't loop
    }

    private void InitializeSounds(List<Sound> sounds, float volumeMultiplier)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * volumeMultiplier;
            s.source.loop = s.loop;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = musicSounds.Find(sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void PlaySFXIfNotPlaying(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        if (s == null)
        {
            return;
        }

        // Play only if not already playing
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound s = musicSounds.Find(sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void StopSFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        RefreshVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        RefreshVolumes();
    }

    public void RefreshVolumes()
    {
        foreach (Sound s in musicSounds)
        {
            s.source.volume = s.volume * musicVolume;
        }

        foreach (Sound s in sfxSounds)
        {
            s.source.volume = s.volume * sfxVolume;
        }

        // Update task audio volume
        taskAudioSource.volume = sfxVolume;
    }

    private void Start()
    {
            PlayMusic("BG1");
    }

    // Added: Play a specific audio clip for tasks
    public void PlayTaskAudio(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        taskAudioSource.clip = clip;
        taskAudioSource.Play();
    }

    // Added: Stop the currently playing task audio
    public void StopTaskAudio()
    {
        if (taskAudioSource.isPlaying)
        {
            taskAudioSource.Stop();
        }
    }
}
