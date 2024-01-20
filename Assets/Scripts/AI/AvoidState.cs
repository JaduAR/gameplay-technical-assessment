using UnityEngine;

public class AvoidState : State
{
    private Opponent _opponentComponent = null;
    private float _currentTimer = 0.0f;

    public const float AvoidTimer = 10.0f;

    public override void Enter(Avatar agent)
    {
        if (_opponentComponent == null)
        {
            _opponentComponent = agent.GetComponent<Opponent>();
        }
        _currentTimer = 0.0f;
    }

    public override void Update(Avatar agent)
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer > AvoidTimer)
        {
            _opponentComponent.UpdateStateToIdle();
        }
        else
        {
            _opponentComponent.MoveToDesiredLocationFromTarget(2.0f, 1.5f);
        }
    }
}