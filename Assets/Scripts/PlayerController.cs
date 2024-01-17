using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Animator _playerAnim;

    private PlayerControls _controls;
    private PlayerControls.CombatActions _combatControls;

    private bool _queueAttack = false;
    private bool _nextAttackIsP1 = true;

    private const float _MIN_DISTANCE = .5f;

    private PlayerState _state = PlayerState.Idle;

    public enum PlayerState
    {
        Idle,
        Attacking,
        ReturningToIdle,
        IdleBeforeHeavyPunch,
        ChargeHeavyPunch
    }

    void Awake()
    {
        _controls = new PlayerControls();
        _combatControls = _controls.Combat;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    public void QueueAttack()
    {
        _queueAttack = true;
    }

    private void Update()
    {
        switch (_state)
        {
            case PlayerState.ReturningToIdle:
                if (_playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    GameManager.Instance.ResetCombo();
                    GameManager.Instance.DisableHitboxes();
                    _playerAnim.Play("Base Movement");
                    _state = PlayerState.Idle;
                }
                break;
            case PlayerState.Idle:
                if (_queueAttack)
                {
                    _state = PlayerState.Attacking;
                    if (_nextAttackIsP1)
                    {
                        GameManager.Instance.PunchLeft();
                        _playerAnim.Play("Idle to P1");
                    }
                    else
                    {
                        GameManager.Instance.PunchRight();
                        _playerAnim.Play("Idle to P2");
                    }
                    _nextAttackIsP1 = !_nextAttackIsP1;
                    _queueAttack = false;
                }
                else
                {
                    Vector2 movementInput = _combatControls.Movement.ReadValue<Vector2>();

                    //If you're too close to opponent, prevent further movement
                    if (Vector3.Distance(transform.position, _enemy.transform.position) <= _MIN_DISTANCE)
                    {
                        if (movementInput.y >= 0) movementInput.y = 0;
                        movementInput.x = 0;
                    }


                    _playerAnim.SetFloat("StrafeX", movementInput.x);
                    _playerAnim.SetFloat("StrafeZ", movementInput.y);
                }
                break;
            case PlayerState.Attacking:
                if (_playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    if (_queueAttack)
                    {
                        _queueAttack = false;
                        if (GameManager.Instance.GetCombo() >= 2)
                        {
                            if (_nextAttackIsP1) _playerAnim.Play("P2 to Idle");
                            else _playerAnim.Play("P1 to Idle");
                            Debug.Log("Going to Heavy Punch!");
                            _state = PlayerState.IdleBeforeHeavyPunch;
                            GameManager.Instance.ResetCombo();
                            GameManager.Instance.DisableHitboxes();
                        }
                        else
                        {
                            if (_nextAttackIsP1)
                            {
                                GameManager.Instance.PunchLeft();
                                _playerAnim.Play("P2 to P1");
                            }
                            else
                            {
                                GameManager.Instance.PunchRight();
                                _playerAnim.Play("P1 to P2");
                            }
                            _nextAttackIsP1 = !_nextAttackIsP1;
                        }
                    }
                    else
                    {
                        if (_nextAttackIsP1) _playerAnim.Play("P2 to Idle");
                        else _playerAnim.Play("P1 to Idle");
                        _state = PlayerState.ReturningToIdle;
                    }
                }
                break;
            case PlayerState.IdleBeforeHeavyPunch:
                if (_playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    Debug.Log("Idle to Charge!");
                    _playerAnim.Play("Idle to Charge");
                    _state = PlayerState.ChargeHeavyPunch;
                }
                break;
            case PlayerState.ChargeHeavyPunch:
                if (_playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    Debug.Log("Charge to heavy Punch!!");
                    _playerAnim.Play("Charge to Heavy Punch");
                    _state = PlayerState.ReturningToIdle;
                    GameManager.Instance.ChargePunch();
                }
                break;
        }
        transform.LookAt(new Vector3(_enemy.transform.position.x, transform.position.y, _enemy.transform.position.z));
    }
}
