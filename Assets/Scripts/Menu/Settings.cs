using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;

    private SettingsData _settingsData;


    private void Start()
    {
        _settingsData = SaveSystem.LoadSettings();

        _musicVolume.value = _settingsData.VolumeMusic;
        _sfxVolume.value = _settingsData.VolumeSfx;

        _musicVolume.onValueChanged.AddListener(delegate { MusicManager.SetVolume(_musicVolume.value); });
        _sfxVolume.onValueChanged.AddListener(delegate { SoundEffectManager.SetVolume(_sfxVolume.value); });

        _closeButton.onClick.AddListener(CloseSettings);
    }

    private void CloseSettings()
    {
        SaveSystem.SaveSettings(_musicVolume.value, _sfxVolume.value);
        gameObject.SetActive(false);
    }
}
