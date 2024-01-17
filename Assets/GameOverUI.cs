using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void HandleRestartButton()
    {
        GameManager.Instance.ReloadScene();
    }
}
