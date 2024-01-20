using System.Collections;
using UnityEngine;

public class IdleState : State
{
    private Opponent _opponentComponent = null;
    public Coroutine _checkToAttackCoroutine = null;
    private bool _isExiting = false;

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
        _opponentComponent.MoveToDesiredLocationFromTarget(1.0f, 0.5f);
    }

    private IEnumerator Co_CheckToAttack()
    {
        while (_isExiting == false)
        {
            yield return new WaitForSeconds(5.0f);

            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
                if (_opponentComponent.CurrentState == EAIState.IDLE)
                {
                    _opponentComponent.UpdateStateToAttack();
                }
            }
        }
    }
}
