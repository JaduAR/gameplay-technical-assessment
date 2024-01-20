using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Avatar _agentAvatar = null;
    private State _currentState = null;

    private void Awake()
    {
        _agentAvatar = GetComponent<Avatar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == null || _agentAvatar.AreActionsDisabled) return;

        _currentState.Update(_agentAvatar);
    }

    public void ChangeState(State newState)
    {
        if(_currentState == newState) return;

        if (_currentState != null)
        {
            _currentState.Exit(_agentAvatar);
        }

        _currentState = newState;

        _currentState.Enter(_agentAvatar);
    }
}
