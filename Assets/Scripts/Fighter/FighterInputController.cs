using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input controller using Unity's new input system to control the fighter.
/// </summary>
[RequireComponent(typeof(FighterMovement))]
public class FighterInputController : MonoBehaviour
{
    private FighterMovement _fighterMovement;
    private Vector2         _movementInput;

    private void Awake()
    {
        _fighterMovement = GetComponent<FighterMovement>();
    }

    /// <summary>
    /// Processes movement input from the player.
    /// </summary>
    /// <param name="context">Input context containing the value of the movement.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        _fighterMovement.Move(_movementInput);
    }
}