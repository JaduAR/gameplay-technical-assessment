using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    public AssetHolder assetHolder;
    public UIManager uiManager;
    public Opponent opponent;
    public PlayerController player;

    private void Awake()
    {
        if (i != null && i != this)
            Destroy(this);
        else
            i = this;

        player = FindObjectOfType<PlayerController>();
        opponent = FindObjectOfType<Opponent>();
    }

    public void TriggerLevelEnd()
    {
        StartCoroutine(LevelEndDelay());
    }

    private IEnumerator LevelEndDelay()
    {
        yield return new WaitForSeconds(0.3f);
        uiManager.DisplayLevelEnd();
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
