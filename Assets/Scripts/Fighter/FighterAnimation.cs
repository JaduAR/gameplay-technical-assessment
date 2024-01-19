using UnityEngine;

/// <summary>
/// Controls the fighter's animation blending based on movement.
/// </summary>
[RequireComponent(typeof(Animator))]
public class FighterAnimation : MonoBehaviour
{
    private                  Animator        _animator;
    [Tooltip("Animation cross fade duration.")]
    [SerializeField] private float           _crossfadeDuration = 0.2f;
    private FighterState _lastState;

    [Tooltip("Animation blending smooth time. Smaller values mean faster responsiveness, but more snappy movement.")]
    [SerializeField] private float _smoothTime = 0.2f;

    [Tooltip("Adjust this to compensate for the diagonal shift in positive strafeX animation.")]
    [SerializeField] private float _strafeXCompensationFactor;

    private float _currentStrafeXVelocity;
    private float _currentStrafeZVelocity;

    private float _currentStrafeX;
    private float _currentStrafeZ;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Updates the movement parameters in the animator with smoothing.
    /// </summary>
    /// <param name="movementVector">The desired movement vector.</param>
    public void UpdateMovement(Vector2 movementVector)
    {
        var radians = _animator.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        var cosY = Mathf.Cos(radians);
        var sinY = Mathf.Sin(radians);
        var strafeX = movementVector.x * cosY - movementVector.y * sinY;
        var strafeZ = movementVector.x * sinY + movementVector.y * cosY;

        // Apply the compensation factor only when StrafeX is positive to compensate for the shift in the positive StrafeX animation.
        if (strafeX > 0)
            strafeZ -= strafeX * _strafeXCompensationFactor;

        _currentStrafeX = Mathf.SmoothDamp(_currentStrafeX, strafeX, ref _currentStrafeXVelocity, _smoothTime);
        _currentStrafeZ = Mathf.SmoothDamp(_currentStrafeZ, strafeZ, ref _currentStrafeZVelocity, _smoothTime);
        _animator.SetFloat("StrafeX", _currentStrafeX);
        _animator.SetFloat("StrafeZ", _currentStrafeZ);
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

                    case FighterState.Charge:
                    case FighterState.HeavyPunch:
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

                    case FighterState.Charge:
                    case FighterState.HeavyPunch:
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