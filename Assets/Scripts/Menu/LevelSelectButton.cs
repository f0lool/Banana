using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    private LevelConfig _levelConfig;
    [SerializeField] private Button _lockButton;

    private Button _button;

    public Button Button {  get { return _button; } }

    public void Init(LevelConfig levelConfig)
    {
        _button = GetComponent<Button>();
        _levelConfig = levelConfig;

        if (_levelConfig.NeedLevelComplete != null)
            _levelConfig.Unlock = _levelConfig.NeedLevelComplete.Complete;

        if (!_levelConfig.Unlock)
        {
            _lockButton.gameObject.SetActive(true);
        }

        _button.image.sprite = levelConfig.IconLevelSprite;
    }

    public void AddListener(LevelConfig currentLevelConfig)
    {
        _button.onClick.AddListener(() => currentLevelConfig = _levelConfig);
    }

    public LevelConfig GetLevelConfig()
    {
        return _levelConfig;
    }

    public void AddListener(UnityAction call)
    {
        _button.onClick.AddListener(call);
    }

    public void RemoveAllListeners() 
    { 
        _button.onClick.RemoveAllListeners();
    }
}
