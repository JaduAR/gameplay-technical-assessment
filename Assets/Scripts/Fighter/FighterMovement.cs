using UnityEngine;

/// <summary>
/// Handles the actual movement of the fighter in world space.
/// </summary>
public class FighterMovement : MonoBehaviour
{
    [Tooltip("Fighter movement speed.")]
    [SerializeField] private float   _movementSpeed = 1f;
    private                  Vector2 _lastDirection;

    /// <summary>
    /// Moves the fighter.
    /// </summary>
    /// <param name="direction">Movement direction vector</param>
    public void Move(Vector2 direction)
    {
        _lastDirection = direction;
        Vector3 movementVector = new Vector3(direction.x, 0, direction.y) * _movementSpeed * Time.deltaTime;
        transform.Translate(movementVector, Space.World);
    }

    /// <summary>
    /// Gets the last movement direction.
    /// </summary>
    /// <returns>The last direction as a Vector2.</returns>
    public Vector2 GetCurrentDirection()
    {
        return _lastDirection;
    }
}