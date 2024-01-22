using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
    {
    public List<HealthBar> playerHealth;
    public GameObject gameOverIU;

    // Start is called before the first frame update
    void Start()
        {
        foreach(var healthBar in playerHealth)
            healthBar.onHealthChanged.AddListener(OnHealthChanged);
        }

    void OnHealthChanged(int health)
        {
        if (health == 0)
            GameOver();
        }
    
    void GameOver()
        {
        gameOverIU.SetActive(true);
        Time.timeScale = 0;
        }

    }
