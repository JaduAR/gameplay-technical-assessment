using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the game's UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Tooltip("Image that will be faded in and zoomed in.")]
    [SerializeField]
    private Image _fadeZoomImage;

    [Tooltip("Punch button event trigger, will be used to make the fighter punch.")]
    [SerializeField]
    private EventTrigger _punchButtonEventTrigger;

    [Tooltip("Button that restarts the scene, allowing the player to play again.")]
    [SerializeField]
    private Button _playAgainButton;

    [Tooltip("Fight sprite that will be shown on game start.")]
    [SerializeField]
    private Sprite _fightSprite;

    [Tooltip("KO sprite that will be shown on game end.")]
    [SerializeField]
    private Sprite _koSprite;

    [Tooltip("Fighter input controller, needed for triggering input actions through UI.")]
    [SerializeField]
    private FighterInputController _fighterInput;

    private bool _isHeavyPunchInProgress;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += HandleGameStart;
        GameManager.Instance.OnGameEnd += HandleGameEnd;
        _playAgainButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));

        var pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((_) => { _fighterInput.ReleasePunch(); });
        _punchButtonEventTrigger.triggers.Add(pointerUpEntry);

        var pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((_) => OnClick_PunchButton());
        _punchButtonEventTrigger.triggers.Add(pointerDownEntry);

        _fighterInput.OnHeavyPunchReady += HandleHeavyPunchReady;
    }

    private void OnClick_PunchButton()
    {
        _fighterInput.ExecutePunch();
        if (_isHeavyPunchInProgress)
            ShakePunchButton();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= HandleGameStart;
            GameManager.Instance.OnGameEnd -= HandleGameEnd;
        }
        _playAgainButton.onClick.RemoveAllListeners();
        _punchButtonEventTrigger.triggers.Clear();
        _fighterInput.OnHeavyPunchReady -= HandleHeavyPunchReady;
    }

    private void HandleGameStart()
    {
        FadeZoomImage(_fightSprite, () =>
        {
            DOVirtual.DelayedCall(1f, () => _fadeZoomImage.gameObject.SetActive(false));
        });
    }

    /// <summary>
    /// Fades and zooms in the fadeZoom image and assigns the given sprite to it.
    /// </summary>
    /// <param name="sprite">Sprite that will be assigned to the fadeZoom image</param>
    /// <param name="callbackAction">Callback that will be invoked on completion.</param>
    private void FadeZoomImage(Sprite sprite, TweenCallback callbackAction = null)
    {
        _fadeZoomImage.sprite = sprite;
        _fadeZoomImage.gameObject.SetActive(true);
        _fadeZoomImage.color = new Color(_fadeZoomImage.color.r, _fadeZoomImage.color.g, _fadeZoomImage.color.b, 0);
        _fadeZoomImage.transform.localScale = Vector3.zero;
        _fadeZoomImage.DOFade(1, 1);
        _fadeZoomImage.transform.DOScale(Vector3.one, 1).OnComplete(callbackAction);
    }

    private void HandleGameEnd()
    {
        FadeZoomImage(_koSprite, () => _playAgainButton.gameObject.SetActive(true));
        _punchButtonEventTrigger.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles the UI changes of charged attack being ready.
    /// </summary>
    /// <param name="ready">Indicates if heavy punch is ready or not.</param>
    private void HandleHeavyPunchReady(bool ready)
    {
        if (ready)
        {
            ResetPunchButton();
            _punchButtonEventTrigger.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetLoops(-1, LoopType.Yoyo);
            _isHeavyPunchInProgress = true;
        }
        else
            ResetPunchButton();
    }

    /// <summary>
    /// Shakes punch button to indicate charging heavy attack.
    /// </summary>
    private void ShakePunchButton()
    {
        ResetPunchButton();
        _punchButtonEventTrigger.transform.DOShakePosition(0.1f, 20f, 20).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// Resets punch button to normal state.
    /// </summary>
    private void ResetPunchButton()
    {
        _isHeavyPunchInProgress = false;
        _punchButtonEventTrigger.transform.DOKill();
        _punchButtonEventTrigger.transform.localScale = Vector3.one;
    }
}