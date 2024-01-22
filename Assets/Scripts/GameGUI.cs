using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameGUI : Singleton<GameGUI> {
    [SerializeField] private RectTransform healthRect;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private Button button;
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private TextMeshProUGUI newBestTimeLabel;
    [SerializeField] private Image punchIcon;
    private float startWidth;
    private float timeElapsed;

    public void Start() {
        startWidth = healthRect.sizeDelta.x;
        button.onClick.AddListener(TitleScreen);

        EventTrigger trigger = punchIcon.gameObject.GetComponent<EventTrigger>() ?? punchIcon.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry downEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        downEntry.callback.AddListener((data) => { OnPunchIconPress((PointerEventData)data); });
        trigger.triggers.Add(downEntry);

        EventTrigger.Entry upEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        upEntry.callback.AddListener((data) => { OnPunchIconRelease((PointerEventData)data); });
        trigger.triggers.Add(upEntry);
    }

    public void Update()  {
        if (GameManager.Instance.finishedGame)
            return;

        timer.text = Utils.TimeString(timeElapsed);
        timeElapsed += Time.deltaTime;
    }

    public void FinishScreen() {
        finishScreen.gameObject.SetActive(true);

        float prevBestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (timeElapsed < prevBestTime) {
            newBestTimeLabel.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("BestTime", timeElapsed);
        } else {
            newBestTimeLabel.gameObject.SetActive(false);
        }
    }

    public void SetHealthBar(float amount) {
        healthRect.DOSizeDelta(new Vector2(startWidth * amount, healthRect.sizeDelta.y), 0.3f);
    }

    private void TitleScreen() {
        GameAudio.Instance.PlaySFX(GameAudio.Instance.click);
        SceneManager.LoadScene("Title");
    }

    private void OnPunchIconPress(PointerEventData data) {
        if (GameManager.Instance.finishedGame)
            return;

        PlayerController.Instance.Punch();
    }

    private void OnPunchIconRelease(PointerEventData data) {
        if (GameManager.Instance.finishedGame)
            return;

        PlayerController.Instance.ReleasePunch();
    }
}
