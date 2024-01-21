using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 100.0f;

    private float _currentHealth = 0.0f;
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    // Start is called before the first frame update
    void Awake()
    {
        Reset();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth -  damage, 0.0f, _maxHealth);
    }

    public void Reset()
    {
        _currentHealth = _maxHealth;
    }
}