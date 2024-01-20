using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;

    private const string _moveXAnimatorParameter = "StrafeX";
    private const string _moveZAnimatorParameter = "StrafeZ";
    [SerializeField]
    private float _speed = 0.7f;

    [SerializeField]
    private float _botMovementTime = 0.5f;

    private float _minValue = -0.5f;
    private float _maxValue = 0.5f;

    bool _animating=false;

    void Start()
    {
        if (_playerAnimator == null)
        {
            gameObject.GetComponent<Animator>();
        }
    }

    public void DodgePunch()
    {
        if (!_animating)
        {
            float stratX = Random.Range(0, _maxValue);
            float stratZ = Random.Range(_minValue, 0);

            _playerAnimator.SetFloat(_moveXAnimatorParameter, stratX);
            _playerAnimator.SetFloat(_moveZAnimatorParameter, stratZ);

            Invoke("StopBot", _botMovementTime);
            _animating = true;
        }
    }

    void StopBot()
    {
        _playerAnimator.SetFloat(_moveXAnimatorParameter, 0);
        _playerAnimator.SetFloat(_moveZAnimatorParameter, 0);
        _animating = false;
    }

}
