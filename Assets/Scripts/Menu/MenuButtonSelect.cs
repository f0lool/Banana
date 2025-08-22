using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonSelect : MonoBehaviour
{
    private WeaponStats _weaponStats;
    [SerializeField] private GameObject _lockPanel;
    private Button _button;

    public Button Button { get { return _button; } }
    public WeaponStats WeaponStats { get {  return _weaponStats; } }

    public void Init(WeaponStats weaponStats)
    {
        _button = GetComponent<Button>();
        _weaponStats = weaponStats;

        if (_weaponStats.NeedLevelComplete != null)
            _weaponStats.Unlock = _weaponStats.NeedLevelComplete.Complete;

        _button.image.sprite = _weaponStats.IconWeaponSprite;

        if (!_weaponStats.Unlock)
        {
            _lockPanel.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        
    }
}
