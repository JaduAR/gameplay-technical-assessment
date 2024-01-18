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

    private PlayerState _state = PlayerState.Idle;

    private const float _MIN_DISTANCE = .5f;
    
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
            //The state the player goes into if they stop attacking or complete a charge punch.
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

                    //If you're too close to opponent, prevent further movement in any direction other than away.
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
                        //If player is attacking and has hit twice already, then prepare for heavy punch
                        if (GameManager.Instance.GetCombo() >= 2)
                        {
                            if (_nextAttackIsP1) _playerAnim.Play("P2 to Idle");
                            else _playerAnim.Play("P1 to Idle");
                            _state = PlayerState.IdleBeforeHeavyPunch;
                            GameManager.Instance.ResetCombo();
                            GameManager.Instance.DisableHitboxes();
                        }
                        //If player hasn't hit twice, then just swap to P1/P2
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
                    //If no attack queued, return to idle
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
                    _playerAnim.Play("Idle to Charge");
                    _state = PlayerState.ChargeHeavyPunch;
                }
                break;
            case PlayerState.ChargeHeavyPunch:
                if (_playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    _playerAnim.Play("Charge to Heavy Punch");
                    _state = PlayerState.ReturningToIdle;
                    GameManager.Instance.ChargePunch();
                }
                break;
        }
        
        transform.LookAt(new Vector3(_enemy.transform.position.x, transform.position.y, _enemy.transform.position.z));


        //There's a bug where if you Strafe to the right / hold D then the player moves towards the enemy instead of circling them like when you strafe left.
        //I think this is due to the way the StrafeX animation is setup coupled with the player turning to look at the enemy all the time. I found a couple
        //'hacky' solutions to it, like the one below of just rotating but they all came with issues. I think it would need to be changed on the animation for
        //the best solution.

        /*if (_playerAnim.GetFloat("StrafeX") > 0)
        {
            transform.Rotate(new Vector3(0, 40));
        }*/
    }
}
