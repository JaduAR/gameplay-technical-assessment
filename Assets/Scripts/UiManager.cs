using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{

    [SerializeField]
    private Slider _playerHealth;

    [SerializeField]
    private TextMeshProUGUI _playerHealthText;

    [SerializeField]
    private Slider _opponentHealth;

    [SerializeField]
    private TextMeshProUGUI _opponentHealthText;

    [SerializeField]
    private GameObject _gameOverPanel;

    public void Init(float playerHealth,float opponentHealth)
    {
        _playerHealth.value = 1;
        _playerHealthText.text = playerHealth.ToString();

        _opponentHealth.value = 1;
        _opponentHealthText.text = opponentHealth.ToString();

    }

    public void UpdatePlayerHealth(float fillAmount,float health)
    {
        Debug.Log("UpdatePlayerHealth "+fillAmount+" Health "+health);
        _playerHealth.value = fillAmount;
        _playerHealthText.text = health.ToString();
    }

    public void UpdateOpponentHealth(float fillAmount, float health)
    {
        Debug.Log("UpdateOpponentHealth " + fillAmount + " Health " + health);

        _opponentHealth.value = fillAmount;
        _opponentHealthText.text = health.ToString();
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }
    public void ResetUI()
    {
        _gameOverPanel.SetActive(false);
    }

}