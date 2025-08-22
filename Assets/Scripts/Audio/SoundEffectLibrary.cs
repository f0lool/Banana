using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] _soundEffectGroups;
    private Dictionary<string, List<AudioClip>> _soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(SoundEffectGroup soundEffectGroup in _soundEffectGroups)
        {
            _soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (_soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = _soundDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

[System.Serializable]
public struct SoundEffectGroup
{
    public string name;
    public List<AudioClip> audioClips;
}
