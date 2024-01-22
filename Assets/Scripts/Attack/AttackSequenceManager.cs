using UnityEngine;

public class AttackSequenceManager : MonoBehaviour
{
    public static AttackSequenceManager Instance;
    public enum Attack
    {
        Punch1 = 1,
        Punch2 = 2,
        Heavy
    }

    [SerializeField] private float _gameSpeed = 1f;
    
    [Header("References")]
    [SerializeField] private AttackAnimationsBehavior _attackAnimation;
    [SerializeField] private AttackPunchingCalculator _punchingCalculator;
    [SerializeField] private AttackHeavyBehavior _heavyBehavior;
    [SerializeField] private AttackDetectionBehavior _detectionBehavior;
    
    private bool _punching;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        _attackAnimation.UpdatePunching(_punchingCalculator.IsPunching);

        Time.timeScale = _gameSpeed;
    }

    public void ReceiveAttackInput()
    {
        _punchingCalculator.AddPlayerPunchTime();
    }

    public void PunchHappened(Attack punchPerformed)
    {
        bool punchLanded = _detectionBehavior.IsInRangeToAttack;
        bool heavyPerformed = punchPerformed == Attack.Heavy;

        if (heavyPerformed)
        {
            _heavyBehavior.ReceiveHeavyPunch();
            if (punchLanded)
            {
                HealthManager.Instance.ExecuteHeavyDamage();
                SoundManager.Instance.PlayHeavySfx();
            }
        }
        else
        {
            var nextPunch = punchPerformed == Attack.Punch1 ? Attack.Punch2 : Attack.Punch1;
            _attackAnimation.SetNextPunch(nextPunch);

            if (punchLanded)
            {
                _heavyBehavior.ReceivePunch();
                HealthManager.Instance.ExecutePunchDamage(punchPerformed);
                SoundManager.Instance.PlayPunchSfx();
            }   
        }

        if (!punchLanded)
            SoundManager.Instance.PlayMissedSfx();
    }
    
    public void EnterPunchState()
    {
        _punchingCalculator.OnEnterPunchState();
    }

    public void ExitPunchState()
    {
        _punchingCalculator.OnExitPunchState();
    }
}
