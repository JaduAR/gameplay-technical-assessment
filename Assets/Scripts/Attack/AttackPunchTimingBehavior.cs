using UnityEngine;

public class AttackPunchTimingBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AttackSequenceManager.Instance.EnterPunchState();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AttackSequenceManager.Instance.ExitPunchState();
    }
}
