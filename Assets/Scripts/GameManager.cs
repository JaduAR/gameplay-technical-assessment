using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainCam;
    [SerializeField] private GameObject[] _camList;
    [SerializeField] private CanvasGroup _KOScreen;
    [SerializeField] private PunchHitbox _leftHandHitBox;
    [SerializeField] private PunchHitbox _rightHandHitBox;
    [SerializeField] private EnemyAI _enemy;

    [HideInInspector] public static GameManager Instance = null;

    private float _enemyHealth = 100;
    private int _comboCount = 0;

    private const float _HEALTH_REDUCTION_TIME = .2f;
    private const float _END_GAME_CAM_TIME = .5f;
    private const float _KO_FADE_TIME = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void TakeDamage(float damageAmount)
    {
        _enemy.SetAvoid();

        //If this is a regular attack, increase combo counter
        if (damageAmount == 10) IncreaseCombo();

        StartCoroutine(LoseHealthAnimation(_enemyHealth, _enemyHealth -= damageAmount));
        if (_enemyHealth <= 0) {
            StartCoroutine(EndOfGameEvent(damageAmount == 100));
        }
    }

    public void PunchLeft()
    {
        _leftHandHitBox.EnableHitbox(10);
    }

    public void PunchRight()
    {
        _rightHandHitBox.EnableHitbox(10);
    }

    public void ChargePunch()
    {
        _rightHandHitBox.EnableHitbox(100);
    }

    public void DisableHitboxes()
    {
        _leftHandHitBox.DisableHitbox();
        _rightHandHitBox.DisableHitbox();
    }

    public int GetCombo()
    {
        return _comboCount;
    }

    public void IncreaseCombo()
    {
        _comboCount++;
    }

    public void ResetCombo()
    {
        _comboCount = 0;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoseHealthAnimation(float startingHealth, float targHealth)
    {
        if (targHealth < 0) targHealth = 0;
        float timer = 0;
        while (timer <= _HEALTH_REDUCTION_TIME)
        {
            timer += Time.unscaledDeltaTime;
            //TODO: Set health bar to correct spot via Mathf.Lerp(startingHealth, targHealth, timer/_HEALTH_REDUCTION_TIME);
            yield return null;
        }
        //TODO: Set health bar to correct spot via targHealth
    }

    private IEnumerator EndOfGameEvent(bool delay)
    {
        //If finished with uppercut, add a delay for cinematic camera
        if (delay) yield return new WaitForSeconds(.3f);
        Time.timeScale = 0;
        _mainCam.SetActive(false);
        foreach(GameObject cam in _camList)
        {
            AudioManager.Instance.PlayCameraSwapAudio();
            cam.SetActive(true);
            yield return new WaitForSecondsRealtime(_END_GAME_CAM_TIME);
            cam.SetActive(false);
        }

        _mainCam.SetActive(true);

        _KOScreen.interactable = true;
        _KOScreen.blocksRaycasts = true;
        AudioManager.Instance.PlayKOSoundEffect();
        float timer = 0;
        while (timer < _KO_FADE_TIME)
        {
            timer += Time.unscaledDeltaTime;
            _KOScreen.alpha = Mathf.Lerp(0, 1, timer / _KO_FADE_TIME);
            yield return null;
        }

    }
}
