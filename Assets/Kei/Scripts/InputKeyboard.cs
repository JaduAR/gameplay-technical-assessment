using Animancer.FSM;
using Animancer.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputKeyboard : InputCharacter
{

    Vector2 moveVect;
    [Space(20)]
    [SerializeField] SO_Logging logger;
    [SerializeField] StateBase attack;
    [SerializeField, Seconds(Rule = Animancer.Validate.Value.IsNotNegative)]
    float attackInputTimeOut = 0.5f;

    StateMachine<StateBase>.InputBuffer inputBuffer;

    private void Awake()
        {
            inputBuffer = new StateMachine<StateBase>.InputBuffer(Character.StateMachine);
        }
    private void Update()
        {
            UpdateMovement();
        }
    void UpdateMovement()
        {
            Movement = new Vector3(moveVect.x, 0, moveVect.y);
        }
    public void UpdateMovement_InputAction(InputAction.CallbackContext _context)
        {
            if (_context.performed)
                {
                    moveVect = _context.ReadValue<Vector2>();
                }
            else if (_context.canceled)
                {
                    moveVect = default;
                }
        }

    public void UpdateAttack_UI()
        {
            inputBuffer.Buffer(attack, attackInputTimeOut);
            inputBuffer.Update();
        }
    public void UpdateAttack_InputActions(InputAction.CallbackContext _context)
        {
            if (_context.performed)
                {
                    inputBuffer.Buffer(attack, attackInputTimeOut);
                }

            inputBuffer.Update();
        }
    }
