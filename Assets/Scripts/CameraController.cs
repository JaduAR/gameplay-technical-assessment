using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 114

public class CameraController : Singleton<CameraController> {
    [SerializeField] private Transform player;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float verticalOffset = 1.0f;
    [SerializeField] private float smoothSpeed = 5f;

    public void ForceUpdate() {
        Vector3 direction = -player.forward;
        direction.y = 0;

        Vector3 desiredPosition = player.position + direction * distance + Vector3.up * verticalOffset;
        transform.position = desiredPosition; // Set position directly

        Vector3 lookTarget = player.position + Vector3.up * verticalOffset;

        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = targetRotation; // Set rotation directly
    }

    void LateUpdate() {
        if (player == null) return;

        Vector3 direction = -player.forward;
        direction.y = 0;

        Vector3 desiredPosition = player.position + direction * distance + Vector3.up * verticalOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        Vector3 lookTarget = player.position + Vector3.up * verticalOffset;

        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
#pragma warning restore 114
