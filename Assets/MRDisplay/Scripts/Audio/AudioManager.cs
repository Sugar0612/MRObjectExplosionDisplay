using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    private static AudioManager instance;

    public static AudioManager Get()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<AudioManager>();
        }
        return instance;
    }

    public void Load(AudioSource source)
    {
        audioSource = source;
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource?.Play();
    }

    public void UnLoad()
    {
        audioSource?.Stop();
        audioSource = null;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}
