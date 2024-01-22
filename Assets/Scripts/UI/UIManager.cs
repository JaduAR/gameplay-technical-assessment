using System.Collections;
using TMPro;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private Player player;

	[SerializeField] private Button punchButton;
	[SerializeField] private Slider opponentHealthSlider;
	[SerializeField] private GameObject resetText;

	void OnEnable()
	{
		punchButton.onClick.AddListener(OnPunchButtonClicked);
		HealthChangeEvent.Instance.AddListener(OnHealthChanged);
	}

	void OnDisable()
	{
		punchButton.onClick.RemoveListener(OnPunchButtonClicked);
		HealthChangeEvent.Instance.RemoveListener(OnHealthChanged);
	}

    // Start is called before the first frame update
    private void Start()
    {
        
    }

	private void OnPunchButtonClicked()
	{
		// Call the HandleInput method on the current state
		player.CurrentState.HandleInput();
	}

	private void OnHealthChanged(int currentHealth)
	{
		// Update the opponent health slider
		opponentHealthSlider.value = currentHealth;

		// Disable the punch button if the opponent is knocked out
		punchButton.interactable = currentHealth > 0;

		// If the opponent is knocked out, start the coroutine to regenerate health
		if (currentHealth <= 0)
		{
			StartCoroutine(RegenerateHealth());
		}
	}

	// Coroutine to regenerate health of the opponent, after a delay
	IEnumerator RegenerateHealth()
	{
		resetText.gameObject.SetActive(true);

		yield return new WaitForSeconds(2.5f);

		resetText.gameObject.SetActive(false);

		// Invoke the RegenerateHealthEvent to notify listeners to regenerate health
		RegenerateHealthEvent.Instance.Invoke(100);

		opponentHealthSlider.value = 100;

		// Enable the punch button
		punchButton.interactable = true;
	}
}
