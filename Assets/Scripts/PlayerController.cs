using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum AttackState
{
    Idle,
    Attacking,
}

public class PlayerController : MonoBehaviour
{
    public event Action<float, string> _onAttack;    

    [SerializeField] private Animator _opponentAnimator;
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private float _normalAttackDamage = 10f;
    [SerializeField] private float _chargeDuration = 1f;

    public AttackState _currentState { get; private set; }
    public bool _chargeComplete { get; private set; }

    private bool _isPunch1;
    private bool _isPunch2;
    private bool _withinAttackTime;
    private bool _isCharging;
    private float _lastTapTime;
    private float _chargeTimer = 0;

    private PlayerMovement _playerMovement;
    private Animator _animator;

    private void Awake()
    {
        _playerMovement = this.GetComponent<PlayerMovement>();
        _animator = this.GetComponent<Animator>();
        _currentState = AttackState.Idle;
    }

    private void Start()
    {       
        _onAttack += _enemyController.TakeDamage;
    }

    private void OnDestroy()
    {
        _onAttack -= _enemyController.TakeDamage;
    }

    private void Update()
    {
        _playerMovement._initiateMovement = !_isCharging;

        if (_isCharging)
        {
            _chargeTimer += Time.deltaTime;

            if(_chargeTimer >= _chargeDuration)
            {
                _chargeComplete = true;
            }
        }
    }

    public void HandleAttack()
    {
        _currentState = AttackState.Attacking;
        float currentTime = Time.time;
        _withinAttackTime = currentTime - _lastTapTime < 0.5f;
        
        if (_withinAttackTime)
        {
            if ((_isPunch1 && _isPunch2) || (_isPunch2 && !_isPunch1) || (_isPunch1 && !_isPunch2))
            {
                if (_isPunch1)                
                    TriggerSecondNormalAttack();
                
                else if (_isPunch2)                
                    ChargeHeavyAttack();               
            }
        }
        else
        {            
            TriggerFirstNormalAttack();            
        }

        
        _lastTapTime = currentTime;
    }

    public void HandleRelease()
    {                
        if (_isCharging)
        {
            if (_chargeComplete)
                TriggerHeavyAttack();
            else
                _animator.SetTrigger("ExitCharge");

            _isCharging = false;
            _chargeTimer = 0;
        }

        StopCoroutine(ReturnToIdle());
        StartCoroutine(ReturnToIdle());
    }

    public void TriggerFirstNormalAttack()
    {        
        _enemyController._amountAttacked = 0;

        _isPunch1 = true;
        _animator.SetTrigger("IdleToP1");
        _onAttack?.Invoke(_normalAttackDamage, "ReceiveP1");
    }

    public void TriggerSecondNormalAttack()
    {
        _isPunch1 = false;
        _isPunch2 = true;
        _animator.SetTrigger("P1ToP2");
        _onAttack?.Invoke(_normalAttackDamage, "ReceiveP2");              
    }

    private void ChargeHeavyAttack()
    {
        _isPunch1 = true;
        _isPunch2 = false;

        if (_enemyController._amountAttacked >= 2)
        {
            _isCharging = true;
            _chargeTimer = 0;
            _animator.SetTrigger("IdleToCharge");            
        }        
    }

    public void TriggerHeavyAttack()
    {
        _chargeComplete = false;
        _animator.SetTrigger("ReleaseHeavyPunch");
        _onAttack?.Invoke(_enemyController._maxHealth, "ReceiveHeavyPunch");       
        
    }

    private IEnumerator ReturnToIdle()
    {     
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _currentState = AttackState.Idle;
    }
}
