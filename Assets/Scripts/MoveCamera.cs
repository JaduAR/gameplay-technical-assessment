using UnityEngine;

public class MoveCamera : MonoBehaviour      //attached to the camera object
{
    [Header("Turn on flag to use mouse to move camera")]
    [Tooltip("Set to true to move camera with the mouse")]
    public bool UseMouseMovement = false;

    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 180.0f;
    float rotationX = 0;
    float rotationY = 0;

    void Update()
    {
        if (UseMouseMovement == true)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            rotationY += -Input.GetAxis("Mouse X") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -lookXLimit, lookXLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}

