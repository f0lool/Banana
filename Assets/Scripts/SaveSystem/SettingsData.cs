using System;
using UnityEngine;

[Serializable]
public class SettingsData
{
    [Range(0, 1)] public float VolumeMusic;
    [Range(0, 1)] public float VolumeSfx;

    public SettingsData(float volumeMusic, float volumeSfx)
    {
        VolumeMusic = volumeMusic;
        VolumeSfx = volumeSfx;
    }
}
