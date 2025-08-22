using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private Button _menuButton;

    private void Start()
    {
        _menuButton.onClick.AddListener(LoadSceneMenu);
    }

    private void LoadSceneMenu()
    {
        StatsController.ResetScore();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveAllListeners();
    }
}
