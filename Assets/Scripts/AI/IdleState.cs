using System.Collections;
using UnityEngine;

public class IdleState : State
{
    private Opponent _opponentComponent = null;
    public Coroutine _checkToAttackCoroutine = null;
    private WaitForSeconds _attackCheckWait = new WaitForSeconds(5.0f);
    private bool _isExiting = false;

    private const float CHANCE_TO_ATTACK = 0.8f;
    private const float DISTANCE_TO_MOVE_CLOSER = 1.0f;
    private const float DISTANCE_TO_MOVE_AWAY = 0.5f;

    public override void Enter(Avatar agent)
    {
        if (_opponentComponent == null)
        {
            _opponentComponent = agent.GetComponent<Opponent>();
        }

        _isExiting = false;
        _checkToAttackCoroutine = agent.StartCoroutine(Co_CheckToAttack());
    }

    public override void Exit(Avatar agent)
    {
        _isExiting = true;

        agent.StopCoroutine(_checkToAttackCoroutine);
        _checkToAttackCoroutine = null;
    }

    public override void Update(Avatar agent)
    {
        _opponentComponent.MoveToDesiredLocationFromTarget(DISTANCE_TO_MOVE_CLOSER, DISTANCE_TO_MOVE_AWAY);
    }

    private IEnumerator Co_CheckToAttack()
    {
        while (_isExiting == false)
        {
            yield return _attackCheckWait;

            if (Random.Range(0.0f, 1.0f) < CHANCE_TO_ATTACK)
            {
                if (_opponentComponent.CurrentState == EAIState.IDLE)
                {
                    _opponentComponent.UpdateStateToAttack();
                }
            }
        }
    }
}
