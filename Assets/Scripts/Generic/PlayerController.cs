using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public string horizontalInputAxis = "Horizontal";
    [SerializeField]
    public string verticalInputAxis = "Vertical";

    //Movement speed;
    [SerializeField]
    public float movementSpeed = 7f;

    private Animator _animator => GetComponent<Animator>();

    private CharacterController Character => GetComponent<CharacterController>();

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public float GetHorizontalMovementInput()
    {
        //return 0, -1, or 1 exactly(assuming a digital input such as
        //a keyboard or joystick button).

        return (Input.GetAxis(verticalInputAxis)); // Inverted
    }

    public float GetVerticalMovementInput()
    {
        //return 0, -1, or 1 exactly(assuming a digital input such as
        //a keyboard or joystick button).
        return Input.GetAxis(horizontalInputAxis) ; 
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 _velocity = Vector3.zero;

        float _horizontalMovement = GetHorizontalMovementInput();
        float _verticalMovement = GetVerticalMovementInput();

        if (_animator)
        {
            _animator.SetFloat("StrafeX", Mathf.Clamp(_horizontalMovement, -1f, 1f));
            _animator.SetFloat("StrafeZ", Mathf.Clamp(_verticalMovement, -1f, 1f));
        }

        _velocity += transform.right * _horizontalMovement * -1f;
        _velocity += transform.forward * _verticalMovement;

        //If necessary, clamp movement vector to magnitude of 1f;
        if (_velocity.magnitude > 1f)
            _velocity.Normalize();

        return _velocity;
    }

    private Vector3 GetMovementVelocity(){
        //Calculate (normalized) movement direction;
        Vector3 _velocity = GetMovementDirection();

        //Multiply (normalized) velocity with movement speed;
        _velocity *= movementSpeed;

        return _velocity;
    }

    private void Move()
    {
        Vector3 _direction = GetMovementVelocity();

        if (_direction.magnitude >= 0.1f)
        {
            Character.Move(_direction * Time.deltaTime);
        }
    }


}
