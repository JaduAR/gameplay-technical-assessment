using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Fighter))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public string _horizontalInputAxis = "Horizontal";
    [SerializeField]
    public string _verticalInputAxis = "Vertical";

    //Movement speed;
    [SerializeField]
    public float _movementSpeed = 1f;

    private CharacterController _character => GetComponent<CharacterController>();

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public float GetHorizontalMovementInput()
    {
        //return 0, -1, or 1 exactly(assuming a digital input such as
        //a keyboard or joystick button).

        return (Input.GetAxis(_verticalInputAxis)); // Inverted
    }

    public float GetVerticalMovementInput()
    {
        //return 0, -1, or 1 exactly(assuming a digital input such as
        //a keyboard or joystick button).
        return Input.GetAxis(_horizontalInputAxis) ; 
    }

    private Vector3 GetMovementDirection()
    {
        float horizontalMovement = GetHorizontalMovementInput();
        float verticalMovement = GetVerticalMovementInput();

        Fighter fighter = GetComponent<Fighter>();

        if (fighter)
        {
            fighter.SetAnim("StrafeX", Mathf.Clamp(horizontalMovement, -1f, 1f));
            fighter.SetAnim("StrafeZ", Mathf.Clamp(verticalMovement, -1f, 1f));
        }

        Vector3 velocity = Vector3.zero;

        velocity += transform.right * horizontalMovement * -1f;
        velocity += transform.forward * verticalMovement;

        //If necessary, clamp movement vector to magnitude of 1f;
        if (velocity.magnitude > 1f)
            velocity.Normalize();

        return velocity;
    }

    private Vector3 GetMovementVelocity(){
        //Calculate (normalized) movement direction;
        Vector3 velocity = GetMovementDirection();

        //Multiply (normalized) velocity with movement speed;
        velocity *= _movementSpeed;

        return velocity;
    }

    private void Move()
    {
        Vector3 _direction = GetMovementVelocity();

        if (_direction.magnitude >= 0.1f)
        {
            _character.Move(_direction * Time.deltaTime);
        }
    }


}
