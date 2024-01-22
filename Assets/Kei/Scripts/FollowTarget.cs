 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;

    [Space(20)]

    [SerializeField] bool lockOnXPos = false;
    [SerializeField] bool lockOnYPos = false;
    [SerializeField] bool lockOnZPos = false;

    [Space(20)]

    [SerializeField] bool lockOnXRot = false;
    [SerializeField] bool lockOnYRot = false;
    [SerializeField] bool lockOnZRot = false;

    [Space(20)]
    [SerializeField] float smoothTime = 0.3f;
    [SerializeField] Vector3 offset = Vector3.zero;

    Vector3 targetPosition;
    Vector3 targetEulerAngles;
    Vector3 curVel = Vector3.zero;
    Vector3 curVelRot = Vector3.zero;
    private void Update()
        {
            targetPosition = transform.position;
            targetEulerAngles = transform.eulerAngles;

            targetPosition.x = lockOnXPos ? targetToFollow.position.x : transform.position.x;
            targetPosition.y = lockOnYPos ? targetToFollow.position.y : transform.position.y;
            targetPosition.z = lockOnZPos ? targetToFollow.position.z : transform.position.z;


            if(lockOnXRot) targetEulerAngles.x = targetToFollow.eulerAngles.x;
            if(lockOnYRot) targetEulerAngles.y = targetToFollow.eulerAngles.y;
            if(lockOnZRot) targetEulerAngles.z = targetToFollow.eulerAngles.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref curVel, smoothTime);
            
            transform.rotation = Quaternion.Euler( Vector3.SmoothDamp(transform.eulerAngles, targetEulerAngles + offset, ref curVelRot, smoothTime)) ;
        }
}
