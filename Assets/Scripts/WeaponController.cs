using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private WeaponStats _weaponStats;
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _reload;
    private bool _isReload = false;
    private int _currentBulletsInMagazine;
    private float _currentReloadCooldown;

    private PlayerInputSystem _playerInputSystem;
    [SerializeField] private LayerMask _layerMask;

    public Camera cam;

    public int CurrentBulletsInMagazine { get { return _currentBulletsInMagazine;  } }
    public int MagazineSize { get { return _weaponStats.MagazineSize;  } }

    public Action<int, int> OnShooting;
    public Action<int, int> OnReloading;

    public bool IsAutoFire { get { return _weaponStats.IsAutoFire; } }
    private GameObject _muzzleFlash;

    private Vector3 _scale;

    private float _currentCooldownTimeBetweenShots;

    private SpriteRenderer _spriteRenderer;

    public void ChangeWeapon(WeaponStats weaponStats)
    {
        _weaponStats = weaponStats;
        _spriteRenderer.sprite = _weaponStats.WeaponSprite;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentCooldownTimeBetweenShots = 0;
    }

    public void Init(WeaponStats weaponStats, PlayerInputSystem inputSystem)
    {
        ChangeWeapon(weaponStats);
        _currentBulletsInMagazine = weaponStats.MagazineSize;
        _currentReloadCooldown = weaponStats.ReloadCooldown;
        _muzzle.transform.localPosition = weaponStats.MuzzlePosition;

        cam = Camera.main;
        _playerInputSystem = inputSystem;
        _playerInputSystem.Player.Enable();
    }


    public void RotateWeapon(Transform player)
    {
        Vector2 dir = _playerInputSystem.Player.Shoot.ReadValue<Vector2>();

        if(DeviceTypeDetector.CheckDeviceType() == DeviceTypeDetector.DeviceType.Desktop)
        {
            dir = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }

        if (player.transform.localScale.x != 1)
        {
            dir = -dir;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if(DeviceTypeDetector.CheckDeviceType() == DeviceTypeDetector.DeviceType.Desktop)
        {
            if (Mathf.Abs(angle) > 100 && Mathf.Abs(Vector2.Distance(transform.position, cam.ScreenToWorldPoint(Input.mousePosition))) > 3f)
            {
                _scale = player.localScale;
                _scale.x = -_scale.x;
                player.localScale = _scale;
            }
        }
        else if(Mathf.Abs(angle) > 90)
        {
            _scale = player.localScale;
            _scale.x = -_scale.x;
            player.localScale = _scale;
        }
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(DeviceTypeDetector.CheckDeviceType() == DeviceTypeDetector.DeviceType.Desktop)
        {
            if (Vector2.Distance(transform.position, cam.ScreenToWorldPoint(Input.mousePosition)) > 1.25f)
                transform.rotation = rotation;
        }
        else
            transform.rotation = rotation;
    }

    public void Shoot(bool IsFacingRight)
    {
        if(_currentCooldownTimeBetweenShots <= 0)
        {
            SoundEffectManager.Play(_weaponStats.WeaponName);
            _muzzleFlash = Instantiate(_weaponStats.MuzzleFlash, _muzzle.transform.position, _muzzle.transform.rotation, _muzzle.transform);
            Destroy(_muzzleFlash, 0.04f);
            _currentBulletsInMagazine--;
            OnShooting?.Invoke(_currentBulletsInMagazine, _weaponStats.MagazineSize);
            var bullet = Instantiate(_weaponStats.ProjectileStats.ProjectilePrefab, _muzzle.transform.position, _muzzle.transform.rotation);
            if (!IsFacingRight)
            {
                Vector3 scale = new Vector3(bullet.transform.localScale.x, bullet.transform.localScale.y);
                scale.x = -bullet.transform.localScale.x;
                bullet.transform.localScale = scale;
                bullet.GetComponent<Rigidbody2D>().AddForce(-transform.right * _weaponStats.ShootForce, ForceMode2D.Impulse);
            }
            else
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * _weaponStats.ShootForce, ForceMode2D.Impulse);
            _currentCooldownTimeBetweenShots = _weaponStats.CooldownBetweenShots;

            Destroy(bullet, _weaponStats.BulletLifeTime);
        }
    }

    public void WeaponReload()
    {
        if (_currentBulletsInMagazine < _weaponStats.MagazineSize)
        {
            _isReload = true;
            if(_reload != null)
                _reload.SetActive(true);
            if(_currentReloadCooldown <= 0)
            {
                _currentBulletsInMagazine = _weaponStats.MagazineSize;
                _currentReloadCooldown = _weaponStats.ReloadCooldown;
                _isReload = false;
                _reload.SetActive(false);
                OnReloading?.Invoke(_currentBulletsInMagazine, _weaponStats.MagazineSize);
            }
        }
        else
            return;
    }

    public void StopShooting()
    {
        _muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        if (_isReload)
        {
            _currentReloadCooldown -= Time.deltaTime;
        }
        _currentCooldownTimeBetweenShots -= Time.deltaTime;
    }

    public bool CanShoot()
    {
        return !_isReload && _currentBulletsInMagazine > 0;
    }
}
