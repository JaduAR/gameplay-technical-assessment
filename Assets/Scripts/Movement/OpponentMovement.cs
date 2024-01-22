using UnityEngine;
using Random = UnityEngine.Random;

public class OpponentMovement : MonoBehaviour
{
    [SerializeField] private AvatarMovement _avatarMovement;
    [SerializeField] private Transform _player;
    [SerializeField] private float _delayToNextMove = 1f;
    [SerializeField] private float _chanceToIdle = 0.3f;
    [SerializeField] private float _chanceToMoveToPlayer = 0.5f;
    [SerializeField] private Vector3 _playerDir;

    private float _initialDelay = 0f;
    private float _horizontal = 0f;
    private float _vertical = 0f;

    private void Awake()
    {
        _initialDelay = _delayToNextMove;
    }

    private void Update()
    {
        if (GameLoopManager.Instance.GameIsEnded)
            return;
        
        _delayToNextMove -= Time.deltaTime;

        //move forward to player
        _playerDir = (transform.position - _player.transform.position).normalized;

        if (_delayToNextMove < 0f)
        {
            var idleValue = Random.Range(0f, 1f); //trying to be idle
            if (idleValue > _chanceToIdle)
            {
                _horizontal = 0f;
                _vertical = 0f;
            }
            else
            {
                var moveToPlayerValue = Random.Range(0f, 1f); //trying to move towards the player
                if (moveToPlayerValue > _chanceToMoveToPlayer)
                {
                    _horizontal = _playerDir.x;
                    _vertical = _playerDir.z;
                }
                else
                {
                    _horizontal = Random.Range(-1f, 1f);
                    _vertical = Random.Range(-1f, 1f);   
                }
            }
            
            _delayToNextMove = _initialDelay;
        }
        
        _avatarMovement.MoveAvatar(_horizontal, _vertical);
    }
}
