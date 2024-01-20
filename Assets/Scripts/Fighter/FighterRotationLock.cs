using UnityEngine;

/// <summary>
/// Handles the rotation of the fighter to always look at the target fighter.
/// </summary>
[RequireComponent(typeof(Fighter))]
public class FighterRotationLock : MonoBehaviour
{
    //This is assigned manually for the demo purpose. In a full project, there would be a SetTarget() method with a target parameter
    [SerializeField]
    [Tooltip("Target fighter to look at.")]
    private Transform _targetFighter;

    private void Update()
    {
        if (_targetFighter != null)
            LookAtTarget();
    }

    /// <summary>
    /// Makes the fighter look at the target fighter.
    /// </summary>
    private void LookAtTarget()
    {
        var directionToTarget = _targetFighter.position - transform.position;
        directionToTarget.y = 0;
        var lookRotation = Quaternion.LookRotation(directionToTarget);
        lookRotation.x = 0;
        lookRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
}