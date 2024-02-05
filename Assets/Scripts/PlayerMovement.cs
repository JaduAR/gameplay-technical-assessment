using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform _lookTarget;
    public float _lookSpeed = 5f;
    public float _strafeSpeed = 5f;
    public bool _initiateMovement = false;
    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    private void Update()
    {        
        if (_initiateMovement)
            HandleMovement();

        FaceOpponent();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        UpdateAnimatorParameters(movement);
        MovePlayer(movement);
    }

    private void FaceOpponent()
    {
        if (_lookTarget != null)
        {
            Vector3 directionToOpponent = _lookTarget.position - transform.position;
            directionToOpponent.y = 0f;

            if (directionToOpponent.sqrMagnitude > 0.001f)
            {
                Quaternion newRotation = Quaternion.LookRotation(directionToOpponent);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * _lookSpeed);
            }
        }
    }

    private void MovePlayer(Vector3 movement)
    {
        transform.Translate(movement * _strafeSpeed * Time.deltaTime, Space.World);
    }

    private void UpdateAnimatorParameters(Vector3 movement)
    {
        _animator.SetFloat("StrafeX", movement.x);
        _animator.SetFloat("StrafeZ", movement.z);
    }
}
