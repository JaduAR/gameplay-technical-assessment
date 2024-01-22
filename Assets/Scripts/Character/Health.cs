using UnityEngine;

// This class represents the health of an object
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // The maximum health of the object
    private int currentHealth; // The current health of the object

	void OnEnable()
	{
		RegenerateHealthEvent.Instance.AddListener(Heal);
	}

	void OnDisable()
	{
		RegenerateHealthEvent.Instance.RemoveListener(Heal);
	}

    void Start()
    {
		// Initialize the current health to the max health
        currentHealth = maxHealth;
    }

	/// <summary>
	/// Reduces the current health by the damage amount
	/// </summary>
	/// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
		currentHealth -= damage;

		if (currentHealth < 0)
		{
			currentHealth = 0;
		}

		// Invoke the HealthChangeEvent to notify listeners that the health has changed
		HealthChangeEvent.Instance.Invoke(currentHealth);
    }

	// Increases the current health by the amount
    private void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

	/// <summary>
	/// Returns true if the current health is 0
	/// </summary>
    public bool IsKnockedOut()
    {
        return currentHealth <= 0;
    }

	/// <summary>
	/// Returns the current health
	/// </summary>
	public int CurrentHealth
	{
		get { return currentHealth; }
	}
}
