using UnityEngine;

/// <summary>
/// Controller for updating the health bar UI when the entity's health changes.
/// </summary>
public class HealthBarController : MonoBehaviour
{
    [Tooltip("The entity that has health.")]
    [SerializeField] private IHasHealth _healthOwner;

    [Tooltip("The health bar component to update.")]
    [SerializeField] private HealthBar _healthBar;

    private void OnEnable()
    {
        if (_healthOwner != null)
            _healthOwner.OnCurrentHealthChanged += HandleCurrentHealthChanged;
    }

    private void OnDisable()
    {
        if (_healthOwner != null)
            _healthOwner.OnCurrentHealthChanged -= HandleCurrentHealthChanged;
    }

    private void Start()
    {
        if (_healthOwner == null) return;
        _healthBar.SetMaxHealth(_healthOwner.MaximumHealth);
        _healthBar.SetHealth(_healthOwner.CurrentHealth);
    }

    private void HandleCurrentHealthChanged(int currentHealth)
    {
        _healthBar.SetHealth(currentHealth);
    }

    private void OnValidate()
    {
        if (_healthOwner == null && !TryGetComponent(out _healthOwner))
            Debug.LogError("HealthBarController requires a component that implements IHasHealth on the same GameObject.", this);
    }
}