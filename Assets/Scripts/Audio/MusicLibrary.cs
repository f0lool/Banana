using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip[] _gameMusics;

    public AudioClip GetRandomClip()
    {
        return _gameMusics[Random.Range(0, _gameMusics.Length)];
    }

    public AudioClip GetMenuMusic()
    {
        return _backgroundMusic;
    }
}
