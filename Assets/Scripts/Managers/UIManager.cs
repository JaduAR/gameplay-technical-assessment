using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image _playerHealthBar = null;
    [SerializeField]
    private Image _opponentHealthBar = null;
    [SerializeField]
    private GameObject _gameWinRootGo = null;
    [SerializeField]
    private GameObject _koImageGo = null;
    [SerializeField]
    private GameObject _gameWinPopUpGo = null;
    [SerializeField]
    private TextMeshProUGUI _gameEndHeaderText = null;

    static UIManager _instance = null;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void UpdatePlayerHealthBar(float healthPercentage)
    {
        _playerHealthBar.fillAmount = healthPercentage;
    }

    public void UpdateOpponentHealthBar(float healthPercentage)
    {
        _opponentHealthBar.fillAmount = healthPercentage;
    }

    public void ShowGameEnd(bool isPlayerWinner)
    {
        _gameWinRootGo.SetActive(true);
        _koImageGo.SetActive(true);
        _gameWinPopUpGo.SetActive(false);

        if (isPlayerWinner)
        {
            _gameEndHeaderText.text = "Game Win";
        }
        else
        {
            _gameEndHeaderText.text = "Game Lost";
        }
    }

    public void ShowGameEndPopUp()
    {
        _gameWinPopUpGo.SetActive(true);
    }

    public void HideGameEnd()
    {
        _gameWinRootGo.SetActive(false);
        _koImageGo.SetActive(false);
        _gameWinPopUpGo.SetActive(false);
    }
}