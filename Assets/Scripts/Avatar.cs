using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    private Animator _animator = null;
    [SerializeField] 
    private SkinnedMeshRenderer _meshRenderer;
    [SerializeField]
    private List<Attack> _comboAttacks = null;
    [SerializeField]
    private List<SphereCollider> _attackColliders = null;
    [SerializeField]
    private List<GameObject> _handParticles = null;
    [SerializeField]
    private Attack _chargeAttack = null;
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
    private Attack _currentAttack = null;
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
        if (_currentAttack == null) return;
        
        AnimatorStateInfo currentAnimationStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _isCurrentlyTransitioningToIdle = currentAnimationStateInfo.IsName(_currentAttack.IdleAnimationStateName);

        CheckForAttackDamage(currentAnimationStateInfo);
        CheckForNextAnimationToPlay(currentAnimationStateInfo);
    }

    private void CheckForAttackDamage(AnimatorStateInfo animatorStateInfo)
    {
        if (_currentAttack.CanDoDamage == false && _isCurrentlyTransitioningToIdle) return;

        if (_attackColliders != null)
        {
            if (_currentAttack.EnableCollisionAtTime >= 0)
            {
                SphereCollider sphereCollider = _attackColliders[(int)_currentAttack.HandIndex];
                if (sphereCollider.gameObject.activeInHierarchy == false && animatorStateInfo.normalizedTime > _currentAttack.EnableCollisionAtTime)
                {
                    int collisions = Physics.OverlapSphereNonAlloc(sphereCollider.transform.position, sphereCollider.radius, _hitColliders, _opponentLayerMask.value, QueryTriggerInteraction.Collide);

                    if (collisions > 0)
                    {
                        ++_attackHits;
                        sphereCollider.gameObject.SetActive(true);

                        OnDamageDone?.Invoke(_hitColliders[0].gameObject.GetComponentInParent<Avatar>(), _currentAttack.Damage, _attackHits > _comboAttacks.Count, _hitColliders[0].ClosestPoint(sphereCollider.transform.position));
                    }
                }
            }
        }
    }

    private void CheckForNextAnimationToPlay(AnimatorStateInfo animatorStateInfo)
    {
        if (animatorStateInfo.normalizedTime > 1f)
        {
            DisableColliders();

            if (_shouldDoFinalChargeAttack)
            {
                if (_currentAttack != _chargeAttack)
                {
                    _currentAttack = _chargeAttack;
                    _animator.Play(_currentAttack.AnimationStateName);

                    OnAttackStart?.Invoke();
                    EnableHandParticles(true, _currentAttack.HandIndex);

                    if (_currentAttack.NextAttackTransition == null)
                    {
                        _shouldDoFinalChargeAttack = false;
                    }
                }
                else
                {
                    _currentAttack = _currentAttack.NextAttackTransition;
                    _animator.Play(_currentAttack.AnimationStateName);

                    OnAttackStart?.Invoke();
                    EnableHandParticles(true, _currentAttack.HandIndex);
                    _shouldDoFinalChargeAttack = false;
                }
            }
            else if (_shouldCombo && _currentAttack.NextAttackTransition != null && !animatorStateInfo.IsName(_currentAttack.NextAttackTransition.AnimationStateName))
            {
                _lastAttackTime = Time.time;
                _currentAttack = _currentAttack.NextAttackTransition;
                _animator.Play(_currentAttack.AnimationStateName);

                OnAttackStart?.Invoke();
                EnableHandParticles(true, _currentAttack.HandIndex);
            }
            else
            {
                if (!string.IsNullOrEmpty(_currentAttack.IdleAnimationStateName) && _isCurrentlyTransitioningToIdle == false)
                {
                    _animator.Play(_currentAttack.IdleAnimationStateName);
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
        if(_comboAttacks == null || _currentAttack == _chargeAttack) return;

        if (_currentAttack == null)
        {
            _lastAttackTime = Time.time;
            _currentAttack = _comboAttacks[UnityEngine.Random.Range(0, 2)];
            _animator.Play(_currentAttack.AnimationStateName);

            OnAttackStart?.Invoke();
            EnableHandParticles(true, _currentAttack.HandIndex);
        }
        else
        {
            if (Time.time - _lastAttackTime <= 0.5f)
            {
                _shouldCombo = true;
            }
            _shouldDoFinalChargeAttack = _attackHits >= _comboAttacks.Count;
        }
    }

    public void ResetAttack()
    {
        EnableHandParticles(false, EHand.LEFT);
        EnableHandParticles(false, EHand.RIGHT);

        _isCurrentlyTransitioningToIdle = false;
        _shouldCombo = false;
        _currentAttack = null;
        _attackHits = 0;
        _animator.Play("Base Movement");
    }

    public void Reset()
    {
        ResetAttack();
    }
}