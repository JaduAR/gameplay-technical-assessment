using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI element which displays the health of an entity using a slider.
/// </summary>
public class HealthBar : MonoBehaviour
{
    [Tooltip("The UI slider that represents the health bar.")]
    [SerializeField] private Slider _hpSlider;

    /// <summary>
    /// Sets the health value of the health bar.
    /// </summary>
    /// <param name="health">The health value to set.</param>
    public void SetHealth(float health)
    {
        _hpSlider.value = health;
    }

    /// <summary>
    /// Sets the maximum health value of the health bar.
    /// </summary>
    /// <param name="maxHealth">The max health value.</param>
    public void SetMaxHealth(float maxHealth)
    {
        _hpSlider.maxValue = maxHealth;
    }
}