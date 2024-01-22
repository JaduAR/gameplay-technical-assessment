using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Debug")] 
    [SerializeField] private bool _applyDamage = false;
    
    [Header("Health")] 
    [SerializeField] private int _initialHealth = 100;
    [SerializeField] private int _punchHealthDamage = 10;
    [SerializeField] private HealthUIBehavior _playerUIBehavior;
    [SerializeField] private HealthUIBehavior _opponentUIBehavior;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        
        _playerUIBehavior.SetInitialHealth(_initialHealth);
        _opponentUIBehavior.SetInitialHealth(_initialHealth);
    }

    public void ExecuteHeavyDamage()
    {
        if(_applyDamage)
            _opponentUIBehavior.ReduceToZero();
        
        CheckDeath();
    }

    public void ExecutePunchDamage(AttackSequenceManager.Attack punchPerformed)
    {
        if (_applyDamage)
            _opponentUIBehavior.ReduceHealth(_punchHealthDamage);

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_opponentUIBehavior.IsDead())
            GameLoopManager.Instance.OnGameOver();
    }
}
