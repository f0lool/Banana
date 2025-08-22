using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private UIController _uiController;
    private PlayerInputSystem _inputSystem;
    private GameConfig _gameConfig;

    private PlayerHealth _health;
    private PlayerMovement _movement;
    private PlayerWeapon _weapon;

    private void Awake()
    {
        _inputSystem = new PlayerInputSystem();
        _gameConfig = Resources.Load<GameConfig>("GameConfigs/GameConfig");
        _health = GetComponent<PlayerHealth>();
        _movement = GetComponent<PlayerMovement>();
        _weapon = GetComponent<PlayerWeapon>();
        _health.Init(_gameConfig.PlayerStats, _uiController);
        _movement.Init(_gameConfig.PlayerStats, _inputSystem);
        _weapon.Init(_gameConfig.PlayerStats, _uiController, _gameConfig.WeaponStats, _inputSystem);
    }

    public void Won()
    {
        _uiController.OpenWinPanel();
    }
}
