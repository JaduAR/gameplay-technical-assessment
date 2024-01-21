using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Avatar _playerAvatar = null;
    [SerializeField]
    private Avatar _opponentAvatar = null;
    [SerializeField]
    private CameraFollow _cameraFollow = null;
    [SerializeField]
    private List<Transform> _hitVFXs = new List<Transform>();
    [SerializeField]
    private List<Transform> _hitKOVFXs = new List<Transform>();

    private Vector3 _playerStartPosition;
    private Quaternion _playerStartRotation;
    private Vector3 _opponentStartPosition;
    private Quaternion _opponentStartRotation;
    private bool _hasReachedEnd = false;

    private const float CHANCE_TO_AVOID = 0.75f;

    private void Start()
    {
        if (_playerAvatar == null)
        {
            return;
        }

        if (_opponentAvatar == null)
        {
            return;
        }

        _playerAvatar.SetTarget(_opponentAvatar.transform);
        _opponentAvatar.SetTarget(_playerAvatar.transform);

        _cameraFollow.AddTarget(_playerAvatar.transform);
        _cameraFollow.AddTarget(_opponentAvatar.transform);

        Health healthComponent = _playerAvatar.GetComponentInParent<Health>();
        UIManager.Instance.UpdatePlayerHealthBar(healthComponent.HealthPercentage, true);

        healthComponent = _opponentAvatar.GetComponentInParent<Health>();
        UIManager.Instance.UpdateOpponentHealthBar(healthComponent.HealthPercentage, true);

        _playerAvatar.OnDamageDone += OnAvatarTakeDamage;
        _opponentAvatar.OnDamageDone += OnAvatarTakeDamage;

        _playerAvatar.OnAttackStart += OnAvatarAttackStart;
        _opponentAvatar.OnAttackStart += OnAvatarAttackStart;

        _playerStartPosition = _playerAvatar.transform.position;
        _playerStartRotation = _playerAvatar.transform.rotation;
        _opponentStartPosition = _opponentAvatar.transform.position;
        _opponentStartRotation = _opponentAvatar.transform.rotation;
    }

    void ResetGame()
    {
        _playerAvatar.Reset();
        _opponentAvatar.Reset();

        Health healthComponent = _playerAvatar.GetComponentInParent<Health>();
        healthComponent.Reset();

        UIManager.Instance.UpdatePlayerHealthBar(healthComponent.HealthPercentage);

        healthComponent = _opponentAvatar.GetComponentInParent<Health>();
        healthComponent.Reset();

        UIManager.Instance.UpdateOpponentHealthBar(healthComponent.HealthPercentage);

        Opponent opponentComponent = _opponentAvatar.GetComponent<Opponent>();
        if (opponentComponent)
        {
            opponentComponent.Reset();
        }

        _playerAvatar.transform.position = _playerStartPosition;
        _playerAvatar.transform.rotation = _playerStartRotation;
        _opponentAvatar.transform.position = _opponentStartPosition;
        _opponentAvatar.transform.rotation = _opponentStartRotation;

        _playerAvatar.AreActionsDisabled = false;
        _opponentAvatar.AreActionsDisabled = false;

        UIManager.Instance.HideGameEnd();

        Time.timeScale = 1.0f;
        _hasReachedEnd = false;
    }

    private void OnGameEndConditionMet(bool isPlayerWinner)
    {
        _hasReachedEnd = true;

        _playerAvatar.Stop();
        _opponentAvatar.Stop();

        _playerAvatar.AreActionsDisabled = true;
        _opponentAvatar.AreActionsDisabled = true;

        UIManager.Instance.ShowGameEnd(isPlayerWinner);

        StartCoroutine(Co_GameEndDelayed());
    }

    private IEnumerator Co_GameEndDelayed()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        Time.timeScale = 1.0f;

        UIManager.Instance.ShowGameEndPopUp();
    }

    private void OnAvatarAttackStart()
    {
        AudioManager.Instance.PlayRandomAttackSwoosh();
    }

    private void OnAvatarTakeDamage(Avatar avatarToDamage, float damage, bool isKO, Vector3 impactPoint)
    {
        if (_hasReachedEnd) return;

        Health healthComponent = avatarToDamage.GetComponentInParent<Health>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);

            //Handle VFXs and Sound
            if (isKO)
            {
                Time.timeScale = 0.5f;

                if (_hitKOVFXs.Count > 0)
                {
                    PoolManager.Instance.Spawn(_hitKOVFXs[Random.Range(0, _hitKOVFXs.Count)], impactPoint, Quaternion.identity);
                }
                
                AudioManager.Instance.PlayRandomKOSound();
            }
            else
            {
                if (_hitVFXs.Count > 0)
                {
                    PoolManager.Instance.Spawn(_hitVFXs[Random.Range(0, _hitVFXs.Count)], impactPoint, Quaternion.identity);
                }

                AudioManager.Instance.PlayRandomPunchSound();
            }

            if (_opponentAvatar == avatarToDamage)
            {
                if (!isKO)
                {
                    if (healthComponent.CurrentHealth > 0)
                    {
                        //chance to avoid
                        if (Random.Range(0.0f, 1.0f) < CHANCE_TO_AVOID)
                        {
                            Opponent oppenentComponent = _opponentAvatar.GetComponent<Opponent>();
                            if (oppenentComponent != null)
                            {
                                if (oppenentComponent.CurrentState == EAIState.IDLE)
                                {
                                    oppenentComponent.UpdateStateToAvoid();
                                }
                            }
                        }
                    }
                }

                UIManager.Instance.UpdateOpponentHealthBar(healthComponent.HealthPercentage);

                if (healthComponent.CurrentHealth <= 0.0f)
                {
                    OnGameEndConditionMet(true);
                }
            }
            else // Player
            {
                UIManager.Instance.UpdatePlayerHealthBar(healthComponent.HealthPercentage);

                if (healthComponent.CurrentHealth <= 0.0f)
                {
                    OnGameEndConditionMet(false);
                }
            }
        }
    }

    public void OnButtonEvent_Attack()
    {
        if (_playerAvatar == null) return;
        if (_playerAvatar.AreActionsDisabled) return;

        _playerAvatar.Attack();
    }

    public void OnButtonEvent_GameEndContinue()
    {
        ResetGame();
    }

    public void OnButtonEvent_Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}