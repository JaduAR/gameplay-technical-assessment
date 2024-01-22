using UnityEngine;

namespace Game.Assets.Scripts.Character.Movement
{
    public class PlayerMovementStrategy: IMovementStrategy
    {
        private readonly Character _character;
        private readonly float _walkingSpeed;
        private readonly float _runningSpeed;

        public PlayerMovementStrategy(Character character, float walkingSpeed, float runningSpeed)
        {
            _walkingSpeed = walkingSpeed;
            _runningSpeed = runningSpeed;
            _character = character;
        }

        public Vector2 Execute(Vector2 input)
        {
            // Battle mode. Player headed to target
            if (_character.Target == null)
            {
                return ExplorationModeUpdate(input);
            }
            // Exploration mode. Player headed to movement direction
            else
            {
                return BattleModeUpdate(input);
            }
        }

        private Vector2 ExplorationModeUpdate(Vector2 direction)
        {
            var worldDirection = CameraRelativeDirection(direction);

            var rotation = Quaternion.LookRotation(worldDirection, Vector3.up);
            _character.transform.rotation = Quaternion.AngleAxis(rotation.eulerAngles.y, Vector3.up);
            _character.transform.position += _character.transform.forward * _runningSpeed * Time.deltaTime;

            return Vector2.up;
        }

        private Vector2 BattleModeUpdate(Vector2 direction)
        {
            var worldDirection = CameraRelativeDirection(direction);

            // Animation direction calculation relative to player rotation
            var forward = _character.transform.forward;
            var right = _character.transform.right;

            var forwardMovement = Vector3.Dot(worldDirection, forward);
            var rightMovement = Vector3.Dot(worldDirection, right);
            var movementVector = new Vector2(rightMovement, forwardMovement);

            _character.transform.position = _character.transform.position + worldDirection * _walkingSpeed * Time.deltaTime;

            return movementVector;
        }

        private Vector3 CameraRelativeDirection(Vector2 direction)
        {
            //reading the input:
            float horizontalAxis = direction.x;
            float verticalAxis = direction.y;

            //assuming we only using the single camera:
            var camera = Camera.main;

            //camera forward and right vectors:
            var forward = camera.transform.forward;
            var right = camera.transform.right;

            //project forward and right vectors on the horizontal plane (y = 0)
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            //this is the direction in the world space we want to move:
            var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

            return desiredMoveDirection;
        }
    }
}