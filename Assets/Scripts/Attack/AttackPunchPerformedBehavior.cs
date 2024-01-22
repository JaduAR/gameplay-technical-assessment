using UnityEngine;

public class AttackPunchPerformedBehavior : StateMachineBehaviour
{
    [SerializeField] private AttackSequenceManager.Attack _punch;

    private bool _waitingToSent = false;
    private bool _statePunchSent = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _waitingToSent = true;
        // AttackSequenceManager.Instance.PunchHappened(_punch);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_statePunchSent)
            return;
        
        if (_waitingToSent && stateInfo.normalizedTime > 0.5f)
        {
            _statePunchSent = true;
            AttackSequenceManager.Instance.PunchHappened(_punch);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // AttackSequenceManager.Instance.PunchHappened(_punch);
        _waitingToSent = false;
        _statePunchSent = false;
    }
}
