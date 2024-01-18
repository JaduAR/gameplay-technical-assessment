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
        Vector3 directionToTarget = _targetFighter.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
}