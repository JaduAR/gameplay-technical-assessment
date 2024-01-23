using UnityEngine;

// This scripts rotates a GameObject towards a target
public class RotateTowardsTarget : MonoBehaviour
{
    [SerializeField] 
    private float _rotationSpeed = 100f;

    [SerializeField] 
    private Transform _targetTransform;

    private void LookToTarget()
    {
        if (_targetTransform == null) return;

        Vector3 _direction = (_targetTransform.position - transform.position).normalized;

        Quaternion _lookRotation = Quaternion.LookRotation(
                                                                new Vector3(_direction.x,
                                                                                0f,
                                                                                _direction.z)
                                                            );

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                _lookRotation,
                                                Time.deltaTime * _rotationSpeed);
    }

    void Update()
    {
        LookToTarget();
    }

}
