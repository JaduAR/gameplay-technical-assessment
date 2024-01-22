using UnityEngine;

public class AttackPunchingCalculator : MonoBehaviour
{
    [SerializeField] private float _playerPunchTime;

    private float _punchingTime = 0f;
    private bool _isPunching;
    private bool _waitingAnimState = false;

    public bool IsPunching => _isPunching;

    private void Update()
    {
        _punchingTime -= Time.deltaTime;

        if (_punchingTime < 0f)
        {
            _punchingTime = 0f;

            if (_waitingAnimState)
                return;
            
            _isPunching = false;
        }
        else
        {
            _isPunching = true;
        }
    }

    public void OnEnterPunchState()
    {
        _waitingAnimState = true;
    }

    public void OnExitPunchState()
    {
        _waitingAnimState = false;
    }

    public void AddPlayerPunchTime()
    {
        _punchingTime = _playerPunchTime;
    }
}
