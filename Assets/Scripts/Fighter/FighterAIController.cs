using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// AI controller for the fighter, which uses <see cref="FighterMovement"/> to simulate movement.
/// </summary>
[RequireComponent(typeof(Fighter), typeof(FighterMovement))]
public class FighterAIController : MonoBehaviour
{
    private FighterMovement _fighterMovement;

    [Tooltip("Distance at which the AI will stop moving, as it is marked safe from the threat.")]
    [SerializeField] 
    private float _safeDistance = 4f;

    [Tooltip("When AI distance to fighter is smaller than this value, it will get away and circulate around fighter.")]
    [SerializeField] 
    private float _escapeDistance = 2f;

    /// <summary>
    /// Fighter that this AI controller controls.
    /// </summary>
    private Fighter _fighter;

    [Tooltip("Distance to check for a wall behind the fighter.")]
    [SerializeField] private float _wallCheckDistance = 1f;

    [Tooltip("Height of ray cast start point.")]
    [SerializeField] private float _wallCheckRaycastHeight = 0.5f;

    private void Awake()
    {
        _fighter = GetComponent<Fighter>();
        _fighterMovement = GetComponent<FighterMovement>();
    }

    private void Update()
    {
        AvoidBehavior();
    }

    private void AvoidBehavior()
    {
        var distance = Vector3.Distance(transform.position, Fighter.LocalFighter.transform.position);
        var direction = Vector2.zero;

        if (distance < _escapeDistance)
        {
            var backDirection = -_fighter.transform.forward;
            var isWallBehind = Physics.Raycast(_fighter.transform.position + new Vector3(0, _wallCheckRaycastHeight, 0), backDirection, out _, _wallCheckDistance);

            direction = isWallBehind ? GetTangentDirection() : GetAwayDirection();
        }
        else if (distance < _safeDistance)
            direction = GetAwayDirection();
        
        _fighterMovement.Move(direction);
    }

    /// <summary>
    /// Gets a direction that shifts away from the local fighter, attempting to get around them to avoid being trapped in a corner.
    /// </summary>
    /// <returns>Tangent <see cref="Vector2"/> direction</returns>
    private Vector2 GetTangentDirection()
    {
        var awayDirection = GetAwayDirection();
        Vector3 tangentDirection = new Vector2(-awayDirection.y, awayDirection.x);
        return tangentDirection;
    }

    /// <summary>
    /// Gets a direction that shifts away from the local fighter to get as far away as possible.
    /// </summary>
    /// <returns>Shifted <see cref="Vector2"/> direction</returns>
    private Vector2 GetAwayDirection()
    {
        var toOpponent = (Fighter.LocalFighter.transform.position - _fighter.transform.position).normalized;
        return -new Vector2(toOpponent.x, toOpponent.z);
    }

}