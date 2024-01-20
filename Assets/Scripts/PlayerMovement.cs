using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;

    private const string _moveXAnimatorParameter = "StrafeX";
    private const string _moveZAnimatorParameter = "StrafeZ";
    [SerializeField]
    private float _speed = 0.7f;

    private float _cachedX;
    private float _cachedZ;
    private float _minValue = -1;
    private float _maxValue = 1;
    float moveDistance;


    // Start is called before the first frame update
    void Start()
    {
        if (_playerAnimator == null)
        {
            gameObject.GetComponent<Animator>();
        }
        // initialize the difference in distance (input ) to zero
        _cachedX = 0;
        _cachedZ = 0;
        moveDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {

            moveDistance += _speed * Time.deltaTime;
        }

        // forward
        if (Input.GetKey(KeyCode.D))
        {

            if (_playerAnimator.GetFloat(_moveZAnimatorParameter) < _maxValue)
            {
                // move
                _cachedZ += moveDistance;
                _playerAnimator.SetFloat(_moveZAnimatorParameter, _cachedZ);
            }
        }

        // backwards
        if (Input.GetKey(KeyCode.A))

        {
            if (_playerAnimator.GetFloat(_moveZAnimatorParameter) > _minValue)
            {
                // move
                _cachedZ -= moveDistance;
                _playerAnimator.SetFloat(_moveZAnimatorParameter, _cachedZ);
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            _playerAnimator.SetFloat(_moveZAnimatorParameter, 0f);
            _cachedZ = 0;
            moveDistance = 0;
        }


        // up
        if (Input.GetKey(KeyCode.S))
        {

            if (_playerAnimator.GetFloat(_moveXAnimatorParameter) < _maxValue)
            {
                // move
                _cachedX += moveDistance;
                _playerAnimator.SetFloat(_moveXAnimatorParameter, _cachedX);
            }
        }

        // down
        if (Input.GetKey(KeyCode.W))

        {
            if (_playerAnimator.GetFloat(_moveXAnimatorParameter) > _minValue)
            {
                // move
                _cachedX -= moveDistance;
                _playerAnimator.SetFloat(_moveXAnimatorParameter, _cachedX);
            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            _playerAnimator.SetFloat(_moveXAnimatorParameter, 0f);
            _cachedX = 0;
            moveDistance = 0;
        }
    }
}