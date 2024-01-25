using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject InGamePanel;
    [SerializeField]
    private GameObject GameOverPanel;

    private void OnEnable()
    {
        HealthComponent.OnDeath += GameOver;
    }

    private void OnDisable()
    {
        HealthComponent.OnDeath -= GameOver;
    }

    private void OnDestroy()
    {
        HealthComponent.OnDeath -= GameOver;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void GameOver(string tag)
    {
        if (GameOverPanel == null) return;

        InGamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

}
