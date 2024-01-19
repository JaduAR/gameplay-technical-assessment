using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a fighter.
/// </summary>
[RequireComponent(typeof(Animator))]
public class Fighter : MonoBehaviour, IHasHealth
{
    public static Fighter LocalFighter;

    [SerializeField]
    [Tooltip("Set this to true if this is the fighter you'll be controlling through input")]
    private bool _isLocalFighter;

    [SerializeField]
    [Tooltip("The left hand collider that will be used to deal damage to the opponent fighter")]
    private Collider _leftHandCollider;

    [SerializeField]
    [Tooltip("The left hand collider that will be used to deal damage to the opponent fighter")]
    private Collider _rightHandCollider;

    /// <summary>
    /// Current fighter damage rate, used for calculating damage infliction on other fighters.
    /// </summary>
    public int CurrentDamageRate { get; private set; }

    [SerializeField]
    [Tooltip("The maximum health of this fighter. Usually 100")]
    private int _maximumHealth = 100;

    private int _currentHealth;

    /// <inheritdoc/>
    public event Action<int> OnCurrentHealthChanged;

    /// <inheritdoc/>
    public int CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            if (value == _currentHealth) return;
            _currentHealth = value;
            OnCurrentHealthChanged?.Invoke(_currentHealth);
        }
    }
    public int MaximumHealth
    {
        get => _maximumHealth;
        private set => _maximumHealth = value;
    }

    [SerializeField] private FighterState _currentState = FighterState.Idle;
    [SerializeField] private FighterState _lastPunchState = FighterState.Punch2;

    private FighterAnimation _fighterAnimation;

    /// <summary>
    /// The amount of time within which a second punch must be started to be considered a combo.
    /// </summary>
    [SerializeField]
    private float _comboTime = 0.5f;

    private                  bool  _isChargingHeavyPunch = false;
    private                  float _lastPunchTime;
    [Tooltip("Amount of punches needed to initiate a charged attack")]
    [SerializeField] private int   _neededComboPunches = 2;
    private int _currentPunchCombo;

    /// <summary>
    /// Punch lock is helpful for avoiding snappy and unrealistic animations.
    /// e.g. we should wait for Punch 1 to be complete before being able to do Punch 2.
    /// </summary>
    private bool _isPunchLocked;

    /// <summary>
    /// Indicates whether heavy punch was charged and is ready to be executed
    /// </summary>
    private bool _isHeavyPunchReady;

    /// <summary>
    /// Invoked when player punches.
    /// </summary>
    public Action OnPunch;

    private void Awake()
    {
        MaximumHealth = _maximumHealth;
        CurrentHealth = MaximumHealth;
        _fighterAnimation = GetComponent<FighterAnimation>();
        if (_isLocalFighter) LocalFighter = this;
    }

    /// <summary>
    /// Changes the fighter's state.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    private void ChangeState(FighterState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
        _fighterAnimation.PlayFighterStateAnimation(_currentState);
        UpdateDamageRate();
    }

    /// <summary>
    /// Updates damage rate based on the current fighter state.
    /// </summary>
    private void UpdateDamageRate()
    {
        switch (_currentState)
        {
            case FighterState.Punch1:
            case FighterState.Punch2:
                CurrentDamageRate = 10;
                break;
            case FighterState.HeavyPunch:
                CurrentDamageRate = 100;
                break;
            default:
                CurrentDamageRate = 0;
                break;
        }
    }

    /// <summary>
    /// Initiates a punch action.
    /// </summary>
    public void Punch()
    {
        if (_currentPunchCombo >= _neededComboPunches)
        {
            StartHeavyPunchCharge();
        }
        else if (_isPunchLocked ) return;
        else
        {
            var punchStateToUse = _lastPunchState == FighterState.Punch1? FighterState.Punch2 : FighterState.Punch1;

            if (Time.time - _lastPunchTime > _comboTime)
                ResetCombo();

            ChangeState(punchStateToUse);
            OnPunch?.Invoke();
            _lastPunchState = punchStateToUse;
        }
        _lastPunchTime = Time.time;
    }

    /// <summary>
    /// Starts charging for a heavy punch.
    /// </summary>
    private void StartHeavyPunchCharge()
    {
        _isChargingHeavyPunch = true;
        ResetCombo();
        ChangeState(FighterState.Charge);
    }

    /// <summary>
    /// Records that a punch has landed.
    /// </summary>
    public void PunchLanded()
    {
        switch (_currentState)
        {
            case FighterState.Punch1:
            case FighterState.Punch2:
                _currentPunchCombo++;
                break;
        }
    }

    /// <summary>
    /// Resets current punch combo.
    /// </summary>
    private void ResetCombo()
    {
        _currentPunchCombo = 0;
    }

    /// <summary>
    /// Releases a heavy punch if it's charged.
    /// </summary>
    public void ReleaseHeavyPunch()
    {
        if (!_isChargingHeavyPunch) return;
        _isChargingHeavyPunch = false;
        StartCoroutine(ExecuteHeavyPunch());
    }

    private IEnumerator ExecuteHeavyPunch()
    {
        yield return new WaitUntil(() => _isHeavyPunchReady);
        ChangeState(FighterState.HeavyPunch);
        OnPunch?.Invoke();
        _isHeavyPunchReady = false;
    }

    /// <inheritdoc/>
    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnTakeDamage?.Invoke(damage);
        if (CurrentHealth <= 0)
            Die();
    }

    /// <inheritdoc/>
    public Action<int> OnTakeDamage { get; set; }

    /// <summary>
    /// Enables the damage-dealing hit box of the left hand
    /// </summary>
    public void EnableLeftHandCollider(int damage)
    {
        _leftHandCollider.enabled = true;
    }

    /// <summary>
    /// Enables the damage-dealing hit box of the right hand
    /// </summary>
    public void EnableRightHandCollider(int damage)
    {
        _rightHandCollider.enabled = true;
    }

    /// <summary>
    /// Locks punching. Fighter won't be able to punch while punching is locked.
    /// </summary>
    public void LockPunching()
    {
        _isPunchLocked = true;
    }

    /// <summary>
    /// Unlocks punching. Fighter will be able to punch again.
    /// </summary>
    public void UnlockPunching()
    {
        DisableHandColliders();
        ChangeState(FighterState.Idle);
        _isPunchLocked = false;
    }

    /// <summary>
    /// Disables the damage-dealing hit boxes of both hands
    /// </summary>
    public void DisableHandColliders()
    {
        _leftHandCollider.enabled = false;
        _rightHandCollider.enabled = false;
    }

    /// <summary>
    /// Sets heavy punch status to ready after it has finished charging.
    /// </summary>
    public void SetHeavyPunchReady()
    {
        _isHeavyPunchReady = true;
    }

    /// <summary>
    /// Kills the fighter and handles their death.
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
    }

}