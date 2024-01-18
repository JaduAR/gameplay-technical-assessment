using System;
using UnityEngine;

/// <summary>
/// Represents a fighter.
/// </summary>
[RequireComponent(typeof(Animator))]
public class Fighter : MonoBehaviour, IHasHealth
{
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

    private Animator _animator;
    public  bool     IsAttacking { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        MaximumHealth = _maximumHealth;
        CurrentHealth = MaximumHealth;
    }

    /// <inheritdoc/>
    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (CurrentHealth <= 0)
            Die();
    }

    /// <summary>
    /// Kills the fighter and handles their death.
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
    }

    private void Update()
    {
        IsAttacking = _animator.GetBool("IsAttacking");
    }
}