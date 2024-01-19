using UnityEngine;

/// <summary>
/// Handles the input for the fighter's movement, setting parameters for the Animator.
/// </summary>
[RequireComponent(typeof(FighterAnimation))]
public class FighterMovement : MonoBehaviour
{
    [Tooltip("Fighter movement speed.")]
    [SerializeField] private float _movementSpeed = 1f;

    private FighterAnimation _fighterAnimation;

    private void Start()
    {
        _fighterAnimation = GetComponent<FighterAnimation>();
    }

    /// <summary>
    /// Updates the movement parameters in the fighter's animation.
    /// </summary>
    /// <param name="direction">Movement direction vector</param>
    public void Move(Vector2 direction)
    {
        _fighterAnimation.UpdateMovement(direction * _movementSpeed);
    }
}