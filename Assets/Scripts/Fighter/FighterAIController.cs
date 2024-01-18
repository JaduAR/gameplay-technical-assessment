using UnityEngine;

/// <summary>
/// AI controller for the fighter, which uses <see cref="FighterMovement"/> to simulate movement.
/// </summary>
[RequireComponent(typeof(FighterMovement))]
public class FighterAIController : MonoBehaviour
{
    private FighterMovement _fighterMovement;

    private void Start()
    {
        _fighterMovement = GetComponent<FighterMovement>();
    }

    private void Update()
    {
        AvoidBehavior();
    }

    /// <summary>
    /// Simulates avoidance behavior for the AI-controlled fighter.
    /// </summary>
    private void AvoidBehavior()
    {
        Vector2 strafeDirection = CalculateStrafeDirection();
        _fighterMovement.Move(strafeDirection);
    }

    /// <summary>
    /// Calculates the direction in which the AI-controlled fighter should strafe.
    /// </summary>
    /// <returns>The calculated strafe direction as a Vector2.</returns>
    private Vector2 CalculateStrafeDirection()
    {
        return Vector2.right; //TODO: Change this
    }
}