using Animancer.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animancer.Validate;

public sealed class CharacterLocomotion : MonoBehaviour
    {
        [SerializeField] private Character _Character;
        [SerializeField] private CharacterController _CharacterController;

        [SerializeField, MetersPerSecond(Rule = Value.IsNotNegative)]
        private float _MaxSpeed = 8;
        public float MaxSpeed => _MaxSpeed;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Acceleration = 20;
        public float Acceleration => _Acceleration;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Deceleration = 25;
        public float Deceleration => _Deceleration;

        [SerializeField, DegreesPerSecond(Rule = Value.IsNotNegative)]
        private float _MinTurnSpeed = 400;
        public float MinTurnSpeed => _MinTurnSpeed;

        [SerializeField, DegreesPerSecond(Rule = Value.IsNotNegative)]
        private float _MaxTurnSpeed = 1200;
        public float MaxTurnSpeed => _MaxTurnSpeed;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Gravity = 20;
        public float Gravity => _Gravity;

        [SerializeField, Multiplier(Rule = Value.IsNotNegative)]
        private float _StickingGravityProportion = 0.3f;
        public float StickingGravityProportion => _StickingGravityProportion;

        public bool IsGrounded { get; private set; }
        public Material GroundMaterial { get; private set; }
        public void UpdateSpeedControl()
        {
            var movement = _Character.Parameters.MovementDirection;

            _Character.Parameters.DesiredForwardSpeed = movement.magnitude * MaxSpeed;

            var deltaSpeed = movement != default ? Acceleration : Deceleration;
            _Character.Parameters.ForwardSpeed = Mathf.MoveTowards(
                _Character.Parameters.ForwardSpeed,
                _Character.Parameters.DesiredForwardSpeed,
                deltaSpeed * Time.deltaTime);
        }
        public float CurrentTurnSpeed
        {
            get
            {
                return Mathf.Lerp(
                    MaxTurnSpeed,
                    MinTurnSpeed,
                    _Character.Parameters.ForwardSpeed / _Character.Parameters.DesiredForwardSpeed);
            }
        }
        public bool GetTurnAngles(Vector3 direction, out float currentAngle, out float targetAngle)
        {
            if (direction == default)
            {
                currentAngle = float.NaN;
                targetAngle = float.NaN;
                return false;
            }

            var transform = this.transform;
            currentAngle = transform.eulerAngles.y;
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            return true;
        }

        public void TurnTowards(float currentAngle, float targetAngle, float speed)
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed * Time.deltaTime);

            transform.eulerAngles = new Vector3(0, currentAngle, 0);
        }

        public void TurnTowards(Vector3 direction, float speed)
        {
            if (GetTurnAngles(direction, out var currentAngle, out var targetAngle))
                TurnTowards(currentAngle, targetAngle, speed);
        }
            private void OnAnimatorMove()
                {
                    Vector3 velocity = _Character.Animancer.Animator.deltaPosition;
                    //velocity.y = _YSpeed * Time.deltaTime;
                    _CharacterController.Move(velocity);
                }

    }
