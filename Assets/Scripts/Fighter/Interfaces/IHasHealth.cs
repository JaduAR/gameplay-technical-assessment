using System;

/// <summary>
/// Interface for entities that have health.
/// </summary>
public interface IHasHealth
{
    /// <summary>
    /// Triggers when current health changes.
    /// </summary>
    event Action<int> OnCurrentHealthChanged;

    /// <summary>
    /// Gets the current health of this entity.
    /// </summary>
    int CurrentHealth { get; }

    /// <summary>
    /// Gets the maximum health of this entity.
    /// </summary>
    int MaximumHealth { get; }

    /// <summary>
    /// Applies damage to this entity.
    /// </summary>
    /// <param name="damage">The amount of damage to be applied to this entity.</param>
    void TakeDamage(int damage);

    /// <summary>
    /// Invoked when entity takes damage
    /// </summary>
    Action<int> OnTakeDamage { get; }
}