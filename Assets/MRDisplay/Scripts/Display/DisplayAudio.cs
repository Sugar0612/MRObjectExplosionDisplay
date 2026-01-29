using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAudio : MonoBehaviour
{
    private static DisplayAudio instance;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Load(AudioSource _audioSource)
    {
        audioSource = _audioSource;
    }

    public void UnLoad()
    {
        audioSource = null;
    }

    public static DisplayAudio Get()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<DisplayAudio>();
        }
        return instance;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}
