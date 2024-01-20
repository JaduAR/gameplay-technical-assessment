using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    PlayerController _player;

    [SerializeField]
    PlayerController _opponent;

    [SerializeField]
    UiManager _UiManager;

    private void Start()
    {
        _player.Init();
        _opponent.Init();

        _player.punchLanded += PunchEnemy;
        _UiManager.Init(_player._playerData.Data.Health, _opponent._playerData.Data.Health);
    }

    private void OnDestroy()
    {
        _player.punchLanded -= PunchEnemy;
    }

    void PunchEnemy(int value)
    {
        _opponent._playerData.CurrentHealth -= value;
        Debug.Log("current health > " + _opponent._playerData.CurrentHealth + " Data Health " + _opponent._playerData.Data.Health);
        var healthBarFillAmount = _opponent._playerData.CurrentHealth / _opponent._playerData.Data.Health;

        _UiManager.UpdateOpponentHealth(healthBarFillAmount, _opponent._playerData.CurrentHealth);

        if (_opponent._playerData.CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        _opponent.PlayDeadAnimation();
        _UiManager.GameOver();
    }

    public void ResetGame()
    {
        _player.ResetData();
        _opponent.ResetData();
        _UiManager.ResetUI();
        _UiManager.Init(_player._playerData.Data.Health, _opponent._playerData.Data.Health);
    }
}