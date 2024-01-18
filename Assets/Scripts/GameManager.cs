using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Manager class with static instance that can be called to conveniently control a variety of imperative game functions. 
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = null;

    [SerializeField] private GameObject _mainCam;
    [SerializeField] private GameObject[] _camList;
    [SerializeField] private CanvasGroup _KOScreen;
    [SerializeField] private PunchHitbox _leftHandHitBox;
    [SerializeField] private PunchHitbox _rightHandHitBox;
    [SerializeField] private EnemyAI _enemy;
    [SerializeField] private RectMask2D _healthBarMask;

    private float _enemyHealth = 100;
    private int _comboCount = 0;

    private float _healthBarMaskWidth;

    private const float _HEALTH_REDUCTION_TIME = .1f;
    private const float _END_GAME_CAM_TIME = .5f;
    private const float _KO_FADE_TIME = 1;

    private void Awake()
    {
        Instance = this;
        _healthBarMaskWidth = _healthBarMask.rectTransform.rect.width;
    }


#region Combat
    //Called to deal damage to enemy
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
    #endregion

    #region Coroutines
    private IEnumerator LoseHealthAnimation(float startingHealth, float targHealth)
    {
        //Because the RectMask2D has to go from 0 to the width, we need the values to go from 0 to 100 instead of the opposite.
        //Multiply by .01f to turn them into a percent. 
        startingHealth = Mathf.Abs(startingHealth - 100) * .01f;
        targHealth = Mathf.Abs(targHealth - 100) * .01f;

        if (targHealth < 0) targHealth = 0;

        float timer = 0;
        while (timer <= _HEALTH_REDUCTION_TIME)
        {
            timer += Time.unscaledDeltaTime;

            //
            _healthBarMask.padding = new Vector4(_healthBarMask.padding.x, _healthBarMask.padding.y, 
                Mathf.Lerp(startingHealth * _healthBarMaskWidth, targHealth * _healthBarMaskWidth, timer/_HEALTH_REDUCTION_TIME),
                _healthBarMask.padding.w);

            yield return null;
        }

        //Set it to targHealth in case it overshot depending on frame length
        _healthBarMask.padding = new Vector4(_healthBarMask.padding.x, _healthBarMask.padding.y,
            targHealth * _healthBarMaskWidth, _healthBarMask.padding.w);
    }

    private IEnumerator EndOfGameEvent(bool delay)
    {
        //If finished with uppercut, add a delay for cinematic camera
        if (delay) yield return new WaitForSeconds(.3f);
        Time.timeScale = 0;
        _mainCam.SetActive(false);
        foreach (GameObject cam in _camList)
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
    #endregion
}
