using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerStats _stats;
    private WeaponStats _weaponStats;
    private WeaponController _weaponController;
    private UIController _uiController;

    private PlayerInputSystem inputActions;

    public void Init(PlayerStats stats, UIController uIController, WeaponStats weaponStats, PlayerInputSystem inputSystem)
    {
        _uiController = uIController;
        _stats = stats;
        _weaponController = GetComponentInChildren<WeaponController>();
        _weaponStats = weaponStats;
        inputActions = new PlayerInputSystem();
        inputActions = inputSystem;
        inputActions.Player.Enable();
    }

    private void Start()
    {
        _weaponController.Init(_weaponStats, inputActions);
        _uiController.ChangeBulletsText(_weaponController.CurrentBulletsInMagazine, _weaponController.MagazineSize);

        _weaponController.OnShooting += _uiController.ChangeBulletsText;
        _weaponController.OnReloading += _uiController.ChangeBulletsText;

        inputActions.Player.Reload.performed += ctx => _weaponController.WeaponReload();
    }

    private void Update()
    {
        _weaponController.RotateWeapon(transform);

        if (_weaponController.CanShoot() && _stats.IsInputEnabled)
        {
                if(DeviceTypeDetector.CheckDeviceType() == DeviceTypeDetector.DeviceType.Desktop)
                {
                    if (_weaponStats.IsAutoFire)
                    {
                        if (Input.GetMouseButton(0))
                            _weaponController.Shoot(transform.localScale.x == 1);
                    } else
                    {
                        if(Input.GetMouseButtonDown(0))
                            _weaponController.Shoot(transform.localScale.x == 1);
                    }
                }
                else if (Mathf.Abs(inputActions.Player.Shoot.ReadValue<Vector2>().x) > 0 || Mathf.Abs(inputActions.Player.Shoot.ReadValue<Vector2>().y) > 0)
                {
                    _weaponController.RotateWeapon(transform);
                    _weaponController.Shoot(transform.localScale.x == 1);
                }

        }else if(!_weaponController.CanShoot())
        {
            _weaponController.WeaponReload();
        }

    }

    private void OnDestroy()
    {
        _weaponController.OnShooting -= _uiController.ChangeBulletsText;
        _weaponController.OnReloading -= _uiController.ChangeBulletsText;

        inputActions.Player.Reload.performed -= ctx => _weaponController.WeaponReload();

        inputActions.Player.Disable();
    }
}
