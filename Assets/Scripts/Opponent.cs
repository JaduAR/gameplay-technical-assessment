using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private Avatar _avatar = null;
    private StateMachine _stateMachine = null;
    private EAIState _currentState = EAIState.IDLE;

    private Dictionary<EAIState, State> _cacheStates = new Dictionary<EAIState, State>();

    public EAIState CurrentState => _currentState;

    // Start is called before the first frame update
    void Start()
    {
        _avatar = GetComponent<Avatar>();
        _stateMachine = GetComponent<StateMachine>();

        _cacheStates.Add(EAIState.IDLE, new IdleState());
        _cacheStates.Add(EAIState.AVOID, new AvoidState());
        _cacheStates.Add(EAIState.ATTACK, new AttackState());

        UpdateStateToIdle();
    }

    public void UpdateStateToIdle()
    {
        _currentState = EAIState.IDLE;
        _stateMachine.ChangeState(_cacheStates[_currentState]);
    }

    public void UpdateStateToAvoid()
    {
        _currentState = EAIState.AVOID;
        _stateMachine.ChangeState(_cacheStates[_currentState]);
    }

    public void UpdateStateToAttack()
    {
        _currentState = EAIState.ATTACK;
        _stateMachine.ChangeState(_cacheStates[_currentState]);
    }

    public void MoveToDesiredLocationFromTarget(float maxDistanceToGetClose, float minDistanceToMoveAway)
    {
        if (_avatar == null) return;

        if (_avatar.AreActionsDisabled)
        {
            _avatar.Move(Vector2.zero);
            return;
        }

        if (_avatar.HasTarget)
        {
            Vector3 fromTarget = _avatar.transform.position - _avatar.CurrentTargetTransform.position;
            float distance = fromTarget.magnitude;

            if (distance > 0)
            {
                fromTarget /= distance;
            }

            if (distance > maxDistanceToGetClose)
            {
                _avatar.Move(new Vector2(fromTarget.x, fromTarget.z));
            }
            else if (distance > 0 && distance < minDistanceToMoveAway)
            {
                _avatar.Move(new Vector2(0f, -fromTarget.z));
            }
            else
            {
                _avatar.Move(Vector2.zero);
            }
        }
        else
        {
            _avatar.Move(Vector2.zero);
        }
    }

    public void Reset()
    {
        UpdateStateToIdle();
    }
}