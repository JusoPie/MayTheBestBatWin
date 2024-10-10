using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager instance;

    // AudioSource for playing sound effects and background music
    public AudioSource sfxSource;
    public AudioSource musicSource;

    // Volume controls
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    // Array to hold sound effect clips (10 slots)
    public AudioClip[] soundEffects = new AudioClip[10];

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the sound manager persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy extra instances
        }
    }

    // Method to play a specific sound effect by index (0 to 9)
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < soundEffects.Length && soundEffects[index] != null)
        {
            sfxSource.PlayOneShot(soundEffects[index], sfxVolume);
        }
        else
        {
            Debug.LogWarning("Sound effect index out of range or clip not assigned.");
        }
    }

    // Method to play background music
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    // Stop the background music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Method to adjust SFX volume
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, 0f, 1f);
    }

    // Method to adjust music volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = musicVolume;
    }
}
