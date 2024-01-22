using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private AvatarBuilder avatarBuilder;
    [SerializeField] private float rotationSpeed = 1.0f;

    [SerializeField] private float _comboTime = 0.5f;
    [SerializeField] private float _chargeTime = 1.5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform enemy;

    private float _elapsedPunchTime;

    private bool punchedLeft = false;
    private bool punchedRight = false;
    private bool charging = false;

    private void LeftPunchAnim(string newAnim) {
        if (_elapsedPunchTime <= _comboTime) {
            avatarBuilder.PlayAnimation(newAnim);
            _elapsedPunchTime = 0f;
        }
    }

    private void RightPunchAnim(string newAnim) {
        if (_elapsedPunchTime <= _comboTime) {
            avatarBuilder.PlayAnimation(newAnim);
            _elapsedPunchTime = 0f;
        }
    }

    private bool SetUpCharge()  {
        if (punchedLeft && punchedRight && _elapsedPunchTime <= _chargeTime) {
            avatarBuilder.SetHoldPunch(true);
            punchedLeft = false;
            punchedRight = false;
            charging = true;
            return true;
        }

        return false;
    }

    public void ReleasePunch() {
        if (charging) {
            avatarBuilder.SetHoldPunch(false);
            charging = false;
        }
    }

    public void Punch() {
        string clip = avatarBuilder.CurrentClip();

        Halt();

        switch (clip) {
            case "Idle to P1":
            case "P2 to P1":
                RightPunchAnim("P1 to P2");
                break;
            case "P1 to Idle":
                RightPunchAnim("Idle to P2");
                break;
            case "Idle to P2":
            case "P1 to P2":
                if (!SetUpCharge()) {
                    LeftPunchAnim("P2 to P1");
                }

                break;
            case "P2 to Idle":
                if (!SetUpCharge()) {
                    LeftPunchAnim("Idle to P1");
                }
  
                break;
            default:
                if (!SetUpCharge()) {
                    avatarBuilder.PlayAnimation("Idle to P1");
                }
                
                _elapsedPunchTime = 0f;
                punchedLeft = false;
                punchedRight = false;
                break;
        }
    }

    void Update() {
        if (GameManager.Instance.finishedGame)
            return;

        _elapsedPunchTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z)) {
            Punch();
        }
        else if (Input.GetKeyUp(KeyCode.Z)) {
            ReleasePunch();
        }
    }

    void FixedUpdate() {
        if (GameManager.Instance.finishedGame)
            return;

        Vector3 directionToPlayer = enemy.transform.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.fixedDeltaTime;
        float horizontal = -Input.GetAxis("Horizontal") * movementSpeed * Time.fixedDeltaTime;
        avatarBuilder.Strafe(vertical, horizontal);
    }

    public void Start() {
        CameraController.Instance.ForceUpdate();
    }

    public void Halt() {
        avatarBuilder.Strafe(0f, 0f);
    }

    public bool LeftPunch() {
        if (punchedLeft || !(avatarBuilder.CurrentClip() == "P2 to P1" || avatarBuilder.CurrentClip() == "Idle to P1")) {
            return false;
        }

        punchedLeft = true;
        return true;
    }

    public bool RightPunch() {
        if (punchedRight || !(avatarBuilder.CurrentClip() == "P1 to P2" || avatarBuilder.CurrentClip() == "Idle to P2")) {
            return false;
        }

        punchedRight = true;
        return true;
    }

    public bool PowerPunch() {
        if (punchedRight || avatarBuilder.CurrentClip() != "Charge to Heavy Punch") {
            return false;
        }

        punchedRight = true;
        return true;
    }
}
