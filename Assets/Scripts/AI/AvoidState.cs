using UnityEngine;

public class AvoidState : State
{
    private Opponent _opponentComponent = null;
    private float _currentTimer = 0.0f;

    public const float AVOID_TIMER = 10.0f;
    private const float DISTANCE_TO_MOVE_CLOSER = 2.0f;
    private const float DISTANCE_TO_MOVE_AWAY = 1.5f;

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

        if (_currentTimer > AVOID_TIMER)
        {
            _opponentComponent.UpdateStateToIdle();
        }
        else
        {
            _opponentComponent.MoveToDesiredLocationFromTarget(DISTANCE_TO_MOVE_CLOSER, DISTANCE_TO_MOVE_AWAY);
        }
    }
}