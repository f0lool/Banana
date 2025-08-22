using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _bulletsText;
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _pausePanel;

    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;

    [Header("PausePanel")]
    [SerializeField] private Button _returnToMenu;
    [SerializeField] private Button _closePauseMenu;

    [SerializeField] private GameObject[] AndroidControl;

    [SerializeField] private TMP_Text _score;

    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;

    private SettingsData _settingsData;

    private void Start()
    {
        _pauseButton.onClick.AddListener(PauseGame);
        _closePauseMenu.onClick.AddListener(UnpauseGame);
        _returnToMenu.onClick.AddListener(ReturnToMenu);

        _settingsData = SaveSystem.LoadSettings();

        _musicVolume.value = _settingsData.VolumeMusic;
        _sfxVolume.value = _settingsData.VolumeSfx;

        _musicVolume.onValueChanged.AddListener(delegate { MusicManager.SetVolume(_musicVolume.value); });
        _sfxVolume.onValueChanged.AddListener(delegate { SoundEffectManager.SetVolume(_sfxVolume.value); });

        if (DeviceTypeDetector.CheckDeviceType() == DeviceTypeDetector.DeviceType.Desktop)
        {
            foreach (var device in AndroidControl)
            {
                device.SetActive(false);
            }
        }

        StatsController.OnScoreChange += ChangeScoreText;
    }

    public void OpenWinPanel()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void OpenLosePanel()
    {
        _losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void SaveVolume()
    {
        SaveSystem.SaveSettings(_musicVolume.value, _sfxVolume.value);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        _pausePanel.SetActive(false);
        SaveSystem.SaveSettings(_musicVolume.value, _sfxVolume.value);
        MusicManager.PlayBackgroundMusicMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void UnpauseGame()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChangeBulletsText(int currentBullet, int maxBullet)
    {
        _bulletsText.text = $"{currentBullet}/{maxBullet}";
    }

    public void ChangePlayerHealthText(int currentHealth, int maxHealth)
    {
        float health = (float)currentHealth / (float)maxHealth;
        _healthBarSlider.value = health;
    }

    public void ChangeScoreText(int score)
    {
        _score.text = $"Очки: {score}";
    }

    private void OnDisable()
    {
        _pauseButton.onClick.RemoveAllListeners();
        _closePauseMenu.onClick.RemoveAllListeners();
        _returnToMenu.onClick.RemoveAllListeners();

        _musicVolume.onValueChanged.RemoveAllListeners();
        _sfxVolume.onValueChanged.RemoveAllListeners();

        StatsController.OnScoreChange -= ChangeScoreText;
    }
}
