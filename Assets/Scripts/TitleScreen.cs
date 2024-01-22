using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : Singleton<TitleScreen> { 
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI timeLabel;

    void Start() {
        GameAudio.Instance.PlayMusic(GameAudio.Instance.titleScreen, true);
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitButton);

        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (bestTime != float.MaxValue) {
            timeLabel.gameObject.SetActive(true);
            timeLabel.text = "Best Time: \n " + Utils.TimeString(bestTime);
        } else {
            timeLabel.gameObject.SetActive(false);
        }
    }

    private void StartGame() {
        SceneManager.LoadScene("Simple Fight");
        GameAudio.Instance.PlaySFX(GameAudio.Instance.click);
    }

    private void ExitButton() {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
