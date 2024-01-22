using UnityEngine;

// This scripts rotates a GameObject towards a target
public class RotateTowardsTarget : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50;
    
    private Vector3 _previousPosition;
    private float _maxRadiansDelta = 1;

    [SerializeField] private Transform _targetTransform;

    void Update()
    {
        if (_targetTransform == null) return;

        Vector3 currentDirection = _targetTransform.position - _previousPosition;

        Vector3 targetDirection = Vector3.RotateTowards(currentDirection, 
                                                        transform.forward, 
                                                        _maxRadiansDelta,
                                                        Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                                                                Quaternion.LookRotation(targetDirection),
                                                                Time.deltaTime * _rotationSpeed);
        
        _previousPosition = transform.position;
    }
}
