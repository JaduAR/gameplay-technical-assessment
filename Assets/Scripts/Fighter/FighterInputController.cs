using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input controller using Unity's new input system to control the fighter.
/// </summary>
[RequireComponent(typeof(FighterMovement), typeof(Fighter))]
public class FighterInputController : MonoBehaviour
{
    private FighterMovement _fighterMovement;
    private Vector2         _movementInput;
    private Fighter         _fighter;

    private void Awake()
    {
        _fighterMovement = GetComponent<FighterMovement>();
        _fighter = GetComponent<Fighter>();
    }

    /// <summary>
    /// Processes movement input from the player.
    /// </summary>
    /// <param name="context">Input context containing the value of the movement.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles the input for punches.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.performed)
            _fighter.Punch();
        
        if (context.canceled)
            _fighter.ReleaseHeavyPunch();
    }

    private void Update()
    {
        _fighterMovement.Move(_movementInput);
    }

}