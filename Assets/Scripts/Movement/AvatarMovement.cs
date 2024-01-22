using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _avatarTransform;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _opponent;

    [Header("Settings")] 
    [SerializeField] private float _movementSpeed = 1f;
    
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _moveDirection;
    private Quaternion _toRotation;

    public void MoveAvatar(float x, float z)
    {
        //setting animations
        _animator.SetFloat("StrafeX", x);
        _animator.SetFloat("StrafeZ", z);
        
        //facing opponent
        _avatarTransform.LookAt(_opponent);
        
        // camera-relative movement
        _forward = _mainCamera.transform.TransformDirection(Vector3.forward);
        _forward.y = 0;
        _forward = _forward.normalized;
        _right = new Vector3(_forward.z, 0, -_forward.x);
        
        // figure out which direction we're going
        _moveDirection = (x * _right + z * _forward).normalized;

        // Actually do the move
        _controller.Move(_moveDirection * (_movementSpeed * Time.deltaTime));
    }
}
