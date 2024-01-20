using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game's UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Tooltip("Image that will be faded in and zoomed in.")]
    [SerializeField]
    private Image _fadeZoomImage;

    [Tooltip("Punch button that will be used to make the fighter punch.")]
    [SerializeField]
    private Button _punchButton;

    [Tooltip("Button that restarts the scene, allowing the player to play again.")]
    [SerializeField]
    private Button _playAgainButton;

    [Tooltip("Fight sprite that will be shown on game start.")]
    [SerializeField]
    private Sprite _fightSprite;

    [Tooltip("KO sprite that will be shown on game end.")]
    [SerializeField]
    private Sprite _koSprite;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += HandleGameStart;
        GameManager.Instance.OnGameEnd += HandleGameEnd;
        _playAgainButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        // Fighter.OnChargedAttackReady += HandleChargedAttackReady;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= HandleGameStart;
            GameManager.Instance.OnGameEnd -= HandleGameEnd;
        }
        _playAgainButton.onClick.RemoveAllListeners();
        // Fighter.OnChargedAttackReady -= HandleChargedAttackReady;

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
        _punchButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles the UI changes of charged attack being ready.
    /// </summary>
    private void HandleChargedAttackReady()
    {
        _punchButton.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // Stops pulsating the punch button.
    public void StopPunchButtonPulsate()
    {
        _punchButton.transform.DOKill();
        _punchButton.transform.localScale = Vector3.one;
    }
}