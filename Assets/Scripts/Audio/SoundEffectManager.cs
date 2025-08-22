using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource _audioSource;
    private static SoundEffectLibrary _soundEffectLibrary;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            _soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = _soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
            _audioSource.PlayOneShot(audioClip);
    }

    public static void Stop()
    {
        _audioSource.Stop();
    }

    public static void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    public static float GetVolume()
    {
        return _audioSource.volume;
    }
}
