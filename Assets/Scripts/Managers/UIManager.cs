using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private HealthBar _playerHealthBar = null;
    [SerializeField]
    private HealthBar _opponentHealthBar = null;
    [SerializeField]
    private GameObject _gameWinRootGo = null;
    [SerializeField]
    private ScaleTo _koImageScaleTo = null;
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

    public void UpdatePlayerHealthBar(float healthPercentage, bool isInstant = false)
    {
        if (isInstant)
        {
            _playerHealthBar.SetValue(healthPercentage);
        }
        else 
        {
            _playerHealthBar.UpdateLerpValueTo(healthPercentage);
        }
    }

    public void UpdateOpponentHealthBar(float healthPercentage, bool isInstant = false)
    {
        if (isInstant)
        {
            _opponentHealthBar.SetValue(healthPercentage);
        }
        else
        {
            _opponentHealthBar.UpdateLerpValueTo(healthPercentage);
        }
    }

    public void ShowGameEnd(bool isPlayerWinner)
    {
        _gameWinRootGo.SetActive(true);

        _koImageScaleTo.gameObject.SetActive(true);
        _koImageScaleTo.Begin();

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
        _koImageScaleTo.gameObject.SetActive(false);
        _gameWinPopUpGo.SetActive(false);
    }
}