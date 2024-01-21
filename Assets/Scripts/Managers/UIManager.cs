using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        //    iTween.ValueTo(gameObject, iTween.Hash(
        //        "from", _playerHealthBar.fillAmount,
        //        "to", healthPercentage,
        //        "onupdate", "UpdatePlayerHealthValueTo",
        //        "time", 0.35f
        //        ));
        }
    }

    //private void UpdatePlayerHealthValueTo(float val)
    //{
    //    _playerHealthBar.fillAmount = val;
    //}

    public void UpdateOpponentHealthBar(float healthPercentage, bool isInstant = false)
    {
        if (isInstant)
        {
            _opponentHealthBar.SetValue(healthPercentage);
            //_opponentHealthBar.fillAmount = healthPercentage;
        }
        else
        {
            _opponentHealthBar.UpdateLerpValueTo(healthPercentage);

            //iTween.ValueTo(gameObject, iTween.Hash(
            //    "from", _opponentHealthBar.fillAmount,
            //    "to", healthPercentage,
            //    "onupdate", "UpdateOpponentHealthValueTo",
            //    "time", 0.35f
            //    ));
        }
    }

    //private void UpdateOpponentHealthValueTo(float val)
    //{
    //    _opponentHealthBar.fillAmount = val;
    //}

    public void ShowGameEnd(bool isPlayerWinner)
    {
        _gameWinRootGo.SetActive(true);

        _koImageScaleTo.gameObject.SetActive(true);
        _koImageScaleTo.Begin();

        //iTween.ScaleTo(_koImageGo, Vector3.one, 0.5f);

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