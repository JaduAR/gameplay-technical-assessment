using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControler : MonoBehaviour      //attached to the UI object
{
    [Header("Player UI Variables")]
    [Tooltip("Player Health Bar Slider")]
    public Slider PlayerHealthBarSlider;

    [Tooltip("Player Name Text")]
    public Text PlayerNameText;

    [Header("Opponent UI Variables")]
    [Tooltip("Opponent Health Bar Slider")]
    public Slider OpponentHealthBarSlider;

    [Tooltip("Opponent Name Text")]
    public Text OpponentNameText;

    [Header("Variables")]
    [Tooltip("Game Data ")]
    public GameData GD;

    private bool VicStarted = false;

    public GameObject FightOverUIGO;

    public GameObject VicBackGroundGO;

    public GameObject KOImageGO;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealthBarSlider.maxValue = GD.PlayerMaxHealth;
        OpponentHealthBarSlider.maxValue = GD.OpponentMaxHealth;

        PlayerNameText.text = GD.PlayerName;

        OpponentNameText.text = GD.OpponentName;

        VicStarted = false;

        VicBackGroundGO.SetActive(false);
        KOImageGO.SetActive(false);
        FightOverUIGO.SetActive(false);
    }

    //Fixed Update called once every frame
    void FixedUpdate()
    {
        UpDateHealthBars();
    }

    //Update Health Bars for both player and Opponent
    public void UpDateHealthBars()
    {
        PlayerHealthBarSlider.value = GD.PlayerHealth;
        OpponentHealthBarSlider.value = GD.OpponentHealth;

        if ((GD.OpponentHealth <= 0) && (!VicStarted))
        {
            VicStarted = true;
            StartVictory();
        }
    }

    public void StartVictory()
    {
        FightOverUIGO.SetActive(true);
        KOImageGO.SetActive(true);
        StartCoroutine(StartKOFade());
    }

    IEnumerator StartKOFade()
    {
        float lerpDuration = 2.0f;
        Color startValue = Color.clear;
        Color endValue = Color.white;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            KOImageGO.GetComponent<Image>().color = Color.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        KOImageGO.GetComponent<Image>().color = endValue;

        yield return new WaitForSeconds(0.5f);
        KOImageGO.SetActive(false);
        VicBackGroundGO.SetActive(true);
    }

    public void RestartButton()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}

