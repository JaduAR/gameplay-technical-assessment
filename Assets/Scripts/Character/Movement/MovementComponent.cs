using System.Collections;
using Game.Assets.Scripts.Character.ActionsBuffer;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Movement
{
    [RequireComponent(typeof(Character), typeof(Animator), typeof(AimComponent))]
    public class MovementComponent: MonoBehaviour
    {
        [SerializeField]
        private float _runningSpeed = 1f;

        [SerializeField]
        private float _walkingSpeed = 1f;

        [SerializeField] 
        private Mode _mode;

        private Character _character;
        private Animator _animator;
        private IObservableActionsBuffer _actionsBuffer;
        private Coroutine _movementCoroutine;
        private IMovementStrategy _movementStrategy;

        public enum Mode
        {
            Player,
            AI
        }

        private void Awake()
        {
            _character = GetComponent<Character>();
            _animator = GetComponent<Animator>();
            _actionsBuffer = _character.ActionsBuffer;

            if (_mode == Mode.AI)
            {
                _movementStrategy = new AiMovementStrategy();
            }
            else
            {
                _movementStrategy = new PlayerMovementStrategy(_character, _walkingSpeed, _runningSpeed);
            }
        }

        private void OnEnable()
        {
            _actionsBuffer.Subscribe<MovementIntent>(MovementStart, MovementEnd);
        }

        private void OnDisable()
        {
            _actionsBuffer.UnSubscribe<MovementIntent>(MovementStart, MovementEnd);
        }

        private void MovementStart(MovementIntent intent)
        {
            if (_movementCoroutine != null)
            {
                Debug.LogError("Previous movement not finished!");
                StopCoroutine(_movementCoroutine);
            }

            _movementCoroutine = StartCoroutine(MovementCoroutine(intent.Direction));
        }

        private void MovementEnd(MovementIntent intent)
        {
            if (_movementCoroutine == null)
                return;

            StopCoroutine(_movementCoroutine);
            SetMoveAnimation(Vector2.zero);
            _movementCoroutine = null;
        }

        private IEnumerator MovementCoroutine(Vector2 direction)
        {
            while (true)
            {
                SetMoveAnimation(_movementStrategy.Execute(direction));
                yield return new WaitForEndOfFrame();
            }
        }

        private void SetMoveAnimation(Vector2 direction)
        {
            _animator.SetFloat("StrafeX", direction.x);
            _animator.SetFloat("StrafeZ", direction.y);
        }
    }
}