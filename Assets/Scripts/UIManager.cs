using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider opponentHealthBar;
    [SerializeField] private GameObject chargeReadyText;
    [SerializeField] private GameObject levelEndPopup;
    [SerializeField] private GameObject attackButton;

    private void Start()
    {
        SetupUI();
    }

    private void SetupUI()
    {
        opponentHealthBar.maxValue = GameManager.i.opponent.Health;
        opponentHealthBar.value = GameManager.i.opponent.Health;
    }

    public void UpdateOpponentHealth()
    {
        opponentHealthBar.value = GameManager.i.opponent.Health;
    }

    public void ChargeReadyTextActive(bool value)
    {
        chargeReadyText.SetActive(value);
    }

    public void DisplayLevelEnd()
    {
        levelEndPopup.SetActive(true);
        attackButton.SetActive(false);
    }
}
