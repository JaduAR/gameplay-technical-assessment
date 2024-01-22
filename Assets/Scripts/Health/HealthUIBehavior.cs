using UnityEngine;
using UnityEngine.UI;

public class HealthUIBehavior : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public bool IsDead() => (int)_slider.value <= 0;

    public void SetInitialHealth(int healthValue)
    {
        _slider.value = healthValue;
    }
    
    public void ReduceHealth(int amount)
    {
        int currentValue = (int)_slider.value;
        currentValue -= amount;

        if (amount <= 0)
        {
            currentValue = 0;
        }

        _slider.value = currentValue;
    }

    public void ReduceToZero()
    {
        _slider.value = 0;
    }
}
