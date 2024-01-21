using System;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    private Animator _animator = null;
    [SerializeField] 
    private SkinnedMeshRenderer _meshRenderer;
    [SerializeField]
    private List<AttackTransition> _comboAttacks = null;
    [SerializeField]
    private List<SphereCollider> _attackColliders = null;
    [SerializeField]
    private List<GameObject> _handParticles = null;
    [SerializeField]
    private AttackTransition _chargeAttack = null;
    [SerializeField]
    private LayerMask _opponentLayerMask;
    [SerializeField]
    private float _movementSpeed = 1.0f;
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    public Action<Avatar, float, bool, Vector3> OnDamageDone;
    public Action OnAttackStart;

    public Transform CurrentTargetTransform => _targetTransform;
    public bool HasTarget => _hasTarget;
    public bool AreActionsDisabled
    {
        get => _disableActions;
        set => _disableActions = value;
    }

    public bool IsMoving => _moveDirection.sqrMagnitude > 0;

    private Transform _targetTransform = null;
    private AttackTransition _currentAttackTransition = null;
    private Vector2 _moveDirection = Vector2.zero;
    private Vector2 _lerpDirection = Vector2.zero;
    private Collider[] _hitColliders = new Collider[1];
    private bool _hasTarget = false;
    private bool _shouldCombo = false;
    private bool _shouldDoFinalChargeAttack = false;
    private bool _isCurrentlyTransitioningToIdle = false;
    private bool _disableActions = false;
    private int _strafeXAnimID = -1;
    private int _strafeYAnimID = -1;
    private int _attackHits = 0;
    private float _lastAttackTime = 0;

    private const string BASE_MOVEMENT_ANIM_NAME = "Base Movement";

    // Start is called before the first frame update
    void Start()
    {
        _strafeXAnimID = Animator.StringToHash("StrafeX");
        _strafeYAnimID = Animator.StringToHash("StrafeZ");

        EnableHandParticles(false, EHand.LEFT);
        EnableHandParticles(false, EHand.RIGHT);
    }

    // Update is called once per frame
    void Update()
    {
        if (!AreActionsDisabled)
        {
            UpdateMovement();
            UpdateRotation();
        }

        HandleAttackTransitions();
    }

    public void Move(Vector2 direction)
    {
        _lerpDirection = direction;
    }

    public void Stop()
    {
        _lerpDirection = Vector2.zero;
        _moveDirection = _lerpDirection;

        _animator.SetFloat(_strafeXAnimID, _moveDirection.x);
        _animator.SetFloat(_strafeYAnimID, _moveDirection.y);
    }

    private void UpdateMovement()
    {
        _moveDirection = Vector2.Lerp(_moveDirection, _lerpDirection, Time.deltaTime * _movementSpeed);

        _animator.SetFloat(_strafeXAnimID, _moveDirection.x);
        _animator.SetFloat(_strafeYAnimID, _moveDirection.y);
    }

    private void UpdateRotation()
    {
        if (_hasTarget == false) return;

        Vector3 toTarget = _targetTransform.position - transform.position;
        toTarget.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toTarget), Time.deltaTime * _rotationSpeed);
    }

    private void HandleAttackTransitions()
    {
        if (_currentAttackTransition == null) return;
        
        AnimatorStateInfo currentAnimationStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _isCurrentlyTransitioningToIdle = currentAnimationStateInfo.IsName(_currentAttackTransition.IdleAnimationStateName);

        CheckForAttackDamage(currentAnimationStateInfo);
        CheckForNextAnimationToPlay(currentAnimationStateInfo);
    }

    //Checks current attack transition to see if it can do damage and if so itll do OverlapSphere test to handle trigger collision of punches
    private void CheckForAttackDamage(AnimatorStateInfo animatorStateInfo)
    {
        if (_currentAttackTransition.CanDoDamage == false || _isCurrentlyTransitioningToIdle) return;

        if (_attackColliders != null)
        {
            if (_currentAttackTransition.EnableCollisionAtTime >= 0)
            {
                SphereCollider sphereCollider = _attackColliders[(int)_currentAttackTransition.HandIndex];
                if (sphereCollider.gameObject.activeInHierarchy == false && animatorStateInfo.normalizedTime > _currentAttackTransition.EnableCollisionAtTime)
                {
                    int collisions = Physics.OverlapSphereNonAlloc(sphereCollider.transform.position, sphereCollider.radius, _hitColliders, _opponentLayerMask.value, QueryTriggerInteraction.Collide);

                    if (collisions > 0)
                    {
                        ++_attackHits;
                        sphereCollider.gameObject.SetActive(true);

                        bool isKO = _currentAttackTransition == _chargeAttack.NextAttackTransition && _attackHits > _comboAttacks.Count;
                        OnDamageDone?.Invoke(_hitColliders[0].gameObject.GetComponentInParent<Avatar>(), _currentAttackTransition.Damage, _attackHits > _comboAttacks.Count, _hitColliders[0].ClosestPoint(sphereCollider.transform.position));
                    }
                }
            }
        }
    }

    //After current animation is complete itll check for next transition either to go to next attack or back to idle
    private void CheckForNextAnimationToPlay(AnimatorStateInfo animatorStateInfo)
    {
        if (animatorStateInfo.normalizedTime > 1f)
        {
            DisableColliders();

            if (_shouldDoFinalChargeAttack)
            {
                if (_currentAttackTransition != _chargeAttack)
                {
                    UpdateAttack(_chargeAttack);
                    
                    if (_currentAttackTransition.NextAttackTransition == null)
                    {
                        _shouldDoFinalChargeAttack = false;
                    }
                }
                else
                {
                    _shouldDoFinalChargeAttack = false;
                    UpdateAttack(_currentAttackTransition.NextAttackTransition);
                }
            }
            else if (_shouldCombo && _currentAttackTransition.NextAttackTransition != null && !animatorStateInfo.IsName(_currentAttackTransition.NextAttackTransition.AnimationStateName))
            {
                _lastAttackTime = Time.time;
                UpdateAttack(_currentAttackTransition.NextAttackTransition);
            }
            else
            {
                if (!string.IsNullOrEmpty(_currentAttackTransition.IdleAnimationStateName) && _isCurrentlyTransitioningToIdle == false)
                {
                    _isCurrentlyTransitioningToIdle = true;
                    _animator.Play(_currentAttackTransition.IdleAnimationStateName);
                }
                else
                {
                    ResetAttack();
                }
            }
        }
    }

    private void DisableColliders()
    {
        if (_attackColliders != null)
        {
            for (int i = 0; i < _attackColliders.Count; ++i)
            {
                _attackColliders[i].gameObject.SetActive(false);
            }
        }
    }

    private void EnableHandParticles(bool enable, EHand handIndex)
    {
        if (_handParticles == null || _handParticles.Count == 0) return;
        if ((int)handIndex > _handParticles.Count) return;

        _handParticles[(int)handIndex].SetActive(enable);
    }

    public void SetTarget(Transform target)
    {
        _targetTransform = target;
        _hasTarget = target != null;
    }

    public void Attack()
    {
        if(_disableActions || _comboAttacks == null || _currentAttackTransition == _chargeAttack.NextAttackTransition) return;

        if (_currentAttackTransition == null)
        {
            _lastAttackTime = Time.time;
            UpdateAttack(_comboAttacks[UnityEngine.Random.Range(0, 2)]);
        }
        else
        {
            if (Time.time - _lastAttackTime <= 0.5f)
            {
                _shouldCombo = true;
            }
            _shouldDoFinalChargeAttack = _attackHits > _comboAttacks.Count - 1 && _chargeAttack != null;
        }
    }

    private void UpdateAttack(AttackTransition nextAttack)
    {
        _currentAttackTransition = nextAttack;
        _animator.Play(_currentAttackTransition.AnimationStateName);

        OnAttackStart?.Invoke();
        EnableHandParticles(true, _currentAttackTransition.HandIndex);
    }

    public void ResetAttack()
    {
        EnableHandParticles(false, EHand.LEFT);
        EnableHandParticles(false, EHand.RIGHT);

        _isCurrentlyTransitioningToIdle = false;
        _shouldCombo = false;
        _currentAttackTransition = null;
        _attackHits = 0;

        _animator.Play(BASE_MOVEMENT_ANIM_NAME);
    }

    public void Reset()
    {
        ResetAttack();
    }
}