using UnityEngine;

/// <summary>
/// Controls the fighter's animation blending based on movement.
/// </summary>
[RequireComponent(typeof(Animator), typeof(FighterMovement))]
public class FighterAnimation : MonoBehaviour
{
    private                  Animator        _animator;
    private                  FighterMovement _fighterMovement;
    [Tooltip("Animation blending smooth time. Smaller values mean faster responsiveness, but more snappy movement.")]
    [SerializeField] private float           _smoothTime = 0.1f;
    private                  float           _velocityX  = 0.0f;
    private                  float           _velocityZ  = 0.0f;

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
}