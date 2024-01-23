using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public string horizontalInputAxis = "Horizontal";
    [SerializeField]
    public string verticalInputAxis = "Vertical";

    //Movement speed;
    [SerializeField]
    public float movementSpeed = 7f;



    CharacterController Character => GetComponent<CharacterController>();

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public float GetHorizontalMovementInput()
    {
        //return 0, -1, or 1 exactly(assuming a digital input such as
        //a keyboard or joystick button).
        return (Input.GetAxisRaw(verticalInputAxis) * - 1f); // Inverted
    }

    public float GetVerticalMovementInput()
    {
        //return 0, -1, or 1 exactly(assuming a digital input such as
        //a keyboard or joystick button).
        return Input.GetAxisRaw(horizontalInputAxis) ; // Inverted
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 _velocity = Vector3.zero;

        _velocity += transform.right * GetHorizontalMovementInput();
        _velocity += transform.forward * GetVerticalMovementInput();

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
