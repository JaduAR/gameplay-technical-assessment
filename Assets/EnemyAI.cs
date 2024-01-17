using UnityEngine;

//Simple AI that will move backwards while taking damage, and will turn to face player.
//Added a cooldown for avoidance to make it possible to charge punch them.
//Also moves back if you're literally right on him.

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private Animator _anim;

    private EnemyState _state = EnemyState.Idle;
    private float _avoidTimer = 0;

    private const float _AVOID_LENGTH = 1;
    private const float _AVOID_COOLDOWN = 3;
    private const float _MIN_DISTANCE = .4f;

    public enum EnemyState
    {
        Idle,
        Avoiding,
        AvoidCooldown
    }

    public void SetAvoid()
    {
        if (_state != EnemyState.AvoidCooldown)
        {
            _avoidTimer = 0;
            _state = EnemyState.Avoiding;
        }
    }

    private void Update()
    {
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));

        switch(_state)
        {
            case EnemyState.Idle:
                MoveIfTooClose();
                break;
            case EnemyState.Avoiding:
                _avoidTimer += Time.deltaTime;
                if (_avoidTimer >= _AVOID_LENGTH)
                {
                    _avoidTimer = 0;
                    _state = EnemyState.AvoidCooldown;
                    _anim.SetFloat("StrafeZ", 0);
                }
                else _anim.SetFloat("StrafeZ", -1);
                break;
            case EnemyState.AvoidCooldown:
                _avoidTimer += Time.deltaTime;
                MoveIfTooClose();
                if (_avoidTimer >= _AVOID_COOLDOWN)
                {
                    _avoidTimer = 0;
                    _state = EnemyState.Idle;
                }
                break;
        }
    }

    private void MoveIfTooClose()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _MIN_DISTANCE) {
            _anim.SetFloat("StrafeZ", -1);
        }
        else _anim.SetFloat("StrafeZ", 0);
    }
}
