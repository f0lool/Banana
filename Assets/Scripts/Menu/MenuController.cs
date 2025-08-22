using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MenuController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteWeapon;
    [SerializeField] private PlayerStats _playerStats;
    private List<MenuButtonSelect> _weaponButtons;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _settingButton;
    private GameConfig _gameConfig;

    [SerializeField] private GameObject _settingsPanel;

    [Header("Level")]
    private LevelConfig[] _levels;
    [SerializeField] private GameObject _levelButtonPrefab;
    [SerializeField] private GameObject _panelChooseLevel;

    [Header("Weapon")]
    private WeaponStats[] _weapons;
    [SerializeField] private GameObject _weaponButtonPrefab;
    [SerializeField] private GameObject _panelChooseWeapon;

    [Header("LevelTypePanel")]
    [SerializeField] private GameObject _panelChooseLevelType;
    [SerializeField] private Button _adventureTypeButton;
    [SerializeField] private Button _deathmatchTypeButton;
    [SerializeField] private LevelConfig[] _deathmatchLevelConfigs;

    private List<LevelSelectButton> _levelSelectButtons;
    private GameData _gameData;
    private SettingsData _settingsData;

    private WeaponStats _currentWeaponStats;
    private LevelConfig _currentLevelConfig;

    private int _currentStageMenu = 0;

    private void Awake()
    {
        _gameConfig = Resources.Load<GameConfig>("GameConfigs/GameConfig");
        _weapons = Resources.LoadAll<WeaponStats>("Weapons");
        _levels = Resources.LoadAll<LevelConfig>("Levels");
        _gameData = SaveSystem.LoadGame();
        _settingsData = SaveSystem.LoadSettings();
        _levelSelectButtons = new List<LevelSelectButton>();
        _weaponButtons = new List<MenuButtonSelect>();
    }

    private void Start()
    {
        if(_currentStageMenu == 0)
            _backButton.gameObject.SetActive(false);

        if (_gameData != null)
        {
            for(int i = 0; i < _levels.Length; i++)
            {
                _levels[i].Complete = _gameData.CompleteLevel[i];
            }
        } else
        {
            SaveSystem.SaveGame(_levels);
            
            _gameData = SaveSystem.LoadGame();
        }

        if(_settingsData != null)
        {
            SoundEffectManager.SetVolume(_settingsData.VolumeSfx);
            MusicManager.SetVolume(_settingsData.VolumeMusic);
        } else
        {
            SaveSystem.SaveSettings(MusicManager.GetVolume(), SoundEffectManager.GetVolume());

            _settingsData = SaveSystem.LoadSettings();
        }

        MusicManager.PlayBackgroundMusicMenu();

        LoadWeaponSelectPanel();
        LoadLevelSelectPanel();
        _currentWeaponStats = _weaponButtons[0].WeaponStats;
        foreach (var button in _weaponButtons)
        {
            button.Button.onClick.AddListener(() => _spriteWeapon.sprite = button.WeaponStats.WeaponSprite);
            button.Button.onClick.AddListener(() => _currentWeaponStats = button.WeaponStats);
        }

        _startButton.onClick.AddListener(StartGame);
        _backButton.onClick.AddListener(BackMenu);
        _adventureTypeButton.onClick.AddListener(ChooseAdventureLevelType);
        _deathmatchTypeButton.onClick.AddListener(ChooseDeathMatchLevelType);
        _settingButton.onClick.AddListener(OpenSettings);

        foreach (var button in _levelSelectButtons)
        {
            button.AddListener(() => _currentLevelConfig = button.GetLevelConfig());
            button.Button.onClick.AddListener(ChooseLevel);
        }

        _panelChooseWeapon.SetActive(false);
        _panelChooseLevel.SetActive(false);
        _startButton.gameObject.SetActive(false);
    }

    private void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }

    private void BackMenu()
    {
        if(_currentStageMenu != 0)
        {
            _currentStageMenu--;
            switch (_currentStageMenu)
            {
                case 0:
                    _backButton.gameObject.SetActive(false);
                    _panelChooseLevelType.SetActive(true);
                    _panelChooseLevel.SetActive(false);
                    _panelChooseWeapon.SetActive(false);
                    _startButton.gameObject.SetActive(false);
                    break;
                case 1:
                    _panelChooseLevelType.SetActive(false);
                    _panelChooseLevel.SetActive(true);
                    _panelChooseWeapon.SetActive(false);
                    _startButton.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void StartGame()
    {
        _gameConfig.WeaponStats = _currentWeaponStats;
        _gameConfig.PlayerStats = _playerStats;
        _gameConfig.LevelConfig = _currentLevelConfig;
        MusicManager.PlayBackgroundMusicGame();
        SceneManager.LoadScene(_currentLevelConfig.NameScene);
    }

    private void ChooseLevel()
    {
        _currentStageMenu++;
        if(_currentStageMenu != 0)
            _backButton.gameObject.SetActive(true);
        _panelChooseLevel.gameObject.SetActive(false);
        _panelChooseWeapon.SetActive(true);
        _startButton.gameObject.SetActive(true);
    }

    private void ChooseAdventureLevelType()
    {
        _currentStageMenu++;
        if (_currentStageMenu != 0)
            _backButton.gameObject.SetActive(true);
        _gameConfig.LevelType = LevelType.Adventure;
        _panelChooseLevelType.SetActive(false);
        _panelChooseLevel.SetActive(true);
    }

    private void ChooseDeathMatchLevelType()
    {
        _currentStageMenu++;
        if (_currentStageMenu != 0)
            _backButton.gameObject.SetActive(true);
        _gameConfig.LevelConfig = _deathmatchLevelConfigs[Random.Range(0, _deathmatchLevelConfigs.Length)];
        _gameConfig.LevelType = LevelType.Deathmatch;
        _currentLevelConfig = _gameConfig.LevelConfig;
        _panelChooseLevelType.SetActive(false);
        _panelChooseWeapon.SetActive(true);
        _startButton.gameObject.SetActive(true);
    }

    private void LoadLevelSelectPanel()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            var levelButton = Instantiate(_levelButtonPrefab, _panelChooseLevel.transform);
            levelButton.GetComponent<LevelSelectButton>().Init(_levels[i]);
            _levelSelectButtons.Add(levelButton.GetComponent<LevelSelectButton>());
        }
    }
    private void LoadWeaponSelectPanel()
    {
        for(int i = 0; i < _weapons.Length; i++)
        {
            var weaponButton = Instantiate(_weaponButtonPrefab, _panelChooseWeapon.transform);
            weaponButton.GetComponent<MenuButtonSelect>().Init(_weapons[i]);
            _weaponButtons.Add(weaponButton.GetComponent<MenuButtonSelect>());
        }
    }

    private void OnDisable()
    {
        foreach(var button in _weaponButtons)
        {
            button.Button.onClick.RemoveAllListeners();
        }

        foreach (var button in _levelSelectButtons)
        {
            button.RemoveAllListeners();
        }

        _startButton.onClick.RemoveAllListeners();
        _adventureTypeButton.onClick.RemoveAllListeners();
        _deathmatchTypeButton.onClick.RemoveAllListeners();
    }
}
