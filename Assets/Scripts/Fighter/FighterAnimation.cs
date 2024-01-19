using UnityEngine;

/// <summary>
/// Controls the fighter's animation blending based on movement.
/// </summary>
[RequireComponent(typeof(Animator), typeof(FighterMovement))]
public class FighterAnimation : MonoBehaviour
{
    private                  Animator        _animator;
    private                  FighterMovement _fighterMovement;
    [Tooltip("Animation cross fade duration.")]
    [SerializeField] private float           _crossfadeDuration = 0.2f;
    [Tooltip("Animation blending smooth time. Smaller values mean faster responsiveness, but more snappy movement.")]
    [SerializeField] private float        _smoothTime = 0.1f;
    private float        _velocityX;
    private float        _velocityZ;
    private FighterState _lastState;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _fighterMovement = GetComponent<FighterMovement>();
    }
    
    private void Update()
    {
        var currentDirection = _fighterMovement.GetCurrentDirection();
        var strafeX = Mathf.SmoothDamp(_animator.GetFloat("StrafeX"), currentDirection.y, ref _velocityZ, _smoothTime);
        var strafeZ = Mathf.SmoothDamp(_animator.GetFloat("StrafeZ"), currentDirection.x, ref _velocityX, _smoothTime);
        _animator.SetFloat("StrafeX", strafeX);
        _animator.SetFloat("StrafeZ", strafeZ);
    }

    /// <summary>
    /// Plays an animation based on <see cref="FighterState"/>
    /// </summary>
    /// <param name="state">The <see cref="FighterState"/> to play the animation based on</param>
    public void PlayFighterStateAnimation(FighterState state)
    {
        switch (state)
        {
            case FighterState.Idle:
            default:
                switch (_lastState)
                {
                    case FighterState.Punch1:

                        _animator.Play("P1 to Idle");

                        break;

                    case FighterState.Punch2:

                        _animator.Play("P2 to Idle");

                        break;

                    default:

                        _animator.CrossFade("Base Movement", _crossfadeDuration);

                        break;

                }
                break;
            case FighterState.Punch1:
                switch (_lastState)
                {
                    case FighterState.Punch2:

                        _animator.Play("P2 to P1");

                        break;

                    default:
                        _animator.CrossFade("Idle to P1", _crossfadeDuration);

                        break;
                }
                break;
            case FighterState.Punch2:
                switch (_lastState)
                {
                    case FighterState.Punch1:

                        _animator.Play("P1 to P2");

                        break;

                    default:
                        _animator.CrossFade("Idle to P2", _crossfadeDuration);

                        break;
                }
                break;
            case FighterState.Charge:
                _animator.Play("Idle to Charge");
                break;

            case FighterState.HeavyPunch:
                _animator.Play("Charge to Heavy Punch");
                break;
        }

        _lastState = state;
    }

}