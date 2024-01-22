using UnityEngine;

public class PlayerInputMovement : MonoBehaviour
{
    [SerializeField] private AvatarMovement _avatarMovement;
    [SerializeField] private AttackPunchingCalculator _punchingCalculator;

    private float _horizontal;
    private float _vertical;

    private void Update()
    {
        _horizontal = 0f;
        _vertical = 0f;
        
        if (GameLoopManager.Instance.GameIsEnded)
            return;
        
        if (_punchingCalculator.IsPunching)
            return;
        
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
        _avatarMovement.MoveAvatar(_horizontal, _vertical);
    }
}
