using System;
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
    /// <summary>
    /// Invoked when player's heavy punch is ready for execution.
    /// </summary>
    public Action<bool> OnHeavyPunchReady;

    private void Awake()
    {
        _fighterMovement = GetComponent<FighterMovement>();
        _fighter = GetComponent<Fighter>();
    }

    private void OnEnable()
    {
        _fighter.OnHeavyPunchReady += HeavyPunchReady;
    }

    private void OnDisable()
    {
        _fighter.OnHeavyPunchReady -= HeavyPunchReady;
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
            ExecutePunch();

        if (context.canceled)
            ReleasePunch();
    }

    /// <summary>
    /// Executes punch.
    /// </summary>
    public void ExecutePunch()
    {
        _fighter.Punch();
    }

    /// <summary>
    /// Releases punch button and attempts to execute heavy punch if ready.
    /// </summary>
    public void ReleasePunch()
    {
        _fighter.ReleaseHeavyPunch();
    }

    /// <summary>
    /// Invoked when heavy punch is ready or executed.
    /// </summary>
    /// <param name="ready">Indicates if heavy punch is ready or not.</param>
    private void HeavyPunchReady(bool ready)
    {
        OnHeavyPunchReady?.Invoke(ready);
    }

    private void Update()
    {
        _fighterMovement.Move(_movementInput);
    }

}