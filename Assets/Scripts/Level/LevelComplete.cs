using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using YG;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelComplete : MonoBehaviour
{
    private GameConfig _gameConfig;
    private LevelConfig[] _levels;

    private void Awake()
    {
        _gameConfig = Resources.Load<GameConfig>("GameConfigs/GameConfig");  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Debug.Log($"Score {YandexGame.savesData.Score}");
            if (StatsController.CurrentScore > YandexGame.savesData.Score)
            {
                YandexGame.savesData.Score = StatsController.CurrentScore;
                YandexGame.NewLeaderboardScores("ScoreBoard", StatsController.CurrentScore);
                YandexGame.SaveProgress();
            }

            if (YandexGame.EnvironmentData.reviewCanShow)
                YandexGame.ReviewShow(true);
            collision.gameObject.GetComponent<Player>().Won();
            _levels = Resources.LoadAll<LevelConfig>("Levels");
            _levels[_gameConfig.LevelConfig.Index - 1].Complete = true;
            SaveSystem.SaveGame(SaveSystem.LoadGame(), _gameConfig.LevelConfig.Index, true);
            SaveSystem.SaveSettings(MusicManager.GetVolume(), SoundEffectManager.GetVolume());
        }
    }
}
