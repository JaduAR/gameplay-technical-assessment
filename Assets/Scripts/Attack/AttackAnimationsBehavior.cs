using UnityEngine;

public class AttackAnimationsBehavior : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AttackButtonBehavior _attackButton;
    
    private static readonly int Punching = Animator.StringToHash("punching");
    private static readonly int Punch = Animator.StringToHash("punch");
    private static readonly int HeavyPunch = Animator.StringToHash("heavyPunch");
    private static readonly int Charging = Animator.StringToHash("charging");

    public void UpdatePunching(bool punching)
    {
        _animator.SetBool(Punching, punching);
    }

    public void SetNextPunch(AttackSequenceManager.Attack nextPunch)
    {
        _animator.SetInteger(Punch, (int)nextPunch);
    }

    private void Update()
    {
        _animator.SetBool(Charging, _attackButton.ButtonPressed);
    }

    public void ChangeHeavyPunch(bool heavyPunchValue)
    {
        _animator.SetBool(HeavyPunch, heavyPunchValue);
    }
}
