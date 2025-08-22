using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        _menuButton.onClick.AddListener(LoadSceneMenu);
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void LoadSceneMenu()
    {
        StatsController.ResetScore();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    private void RestartGame()
    {
        StatsController.ResetScore();
        Time.timeScale = 1.0f;
        MusicManager.PlayBackgroundMusicGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveAllListeners();
    }
}
