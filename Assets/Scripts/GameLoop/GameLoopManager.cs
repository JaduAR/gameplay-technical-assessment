using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance;

    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _endGamePanel;

    private bool _gameIsEnded;

    public bool GameIsEnded => _gameIsEnded;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        
        _endGamePanel.SetActive(false);
        _restartButton.onClick.AddListener(OnRestartClicked);
    }

    public void OnGameOver()
    {
        _gameIsEnded = true;
        _endGamePanel.SetActive(true);
        _restartButton.Select();
    }

    private void OnRestartClicked()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
