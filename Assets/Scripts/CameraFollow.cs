using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float _followDistanceToTarget = 1.0f;
    [SerializeField]
    private float _moveSpeed = 1.0f;

    private List<Transform> _targetFollowTransforms = new List<Transform>();
    private Vector3 _currentPosition = Vector3.zero;
    private float _height = 0.0f;

    private void Start()
    {
        _currentPosition = transform.position;
        _height = _currentPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (_targetFollowTransforms.Count == 0) return;

        Vector3 avgTargetPosition = GetAveragePositionFromTargets();

        Vector3 destinationPosition = avgTargetPosition + Vector3.back * _followDistanceToTarget;
        destinationPosition.y = _height;
        transform.position = Vector3.Lerp(transform.position, destinationPosition, Time.deltaTime * _moveSpeed);

        _currentPosition = transform.position;
    }

    public void AddTarget(Transform target)
    {
        if (_targetFollowTransforms.Contains(target)) return;

        _targetFollowTransforms.Add(target);
    }

    private Vector3 GetAveragePositionFromTargets()
    {
        if(_targetFollowTransforms.Count == 0) return Vector3.zero;
        if (_targetFollowTransforms.Count == 1) return _targetFollowTransforms[0].transform.position;

        Vector3 avgPosition = Vector3.zero;

        for (int i = 0; i < _targetFollowTransforms.Count; ++i)
        {
            avgPosition += _targetFollowTransforms[i].transform.position;
        }

        avgPosition /= _targetFollowTransforms.Count;

        return avgPosition;
    }
}