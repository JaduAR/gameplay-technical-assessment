using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider opponentHealthBar;

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
}
