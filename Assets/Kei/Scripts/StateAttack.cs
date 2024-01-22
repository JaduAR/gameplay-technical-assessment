using Animancer;
using Animancer.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateAttack : StateBase
    {
    

        [SerializeField, DegreesPerSecond] private float _TurnSpeed = 400;
        [SerializeField] private ClipTransition[] _Animations;

        private int _CurrentAnimationIndex = int.MaxValue;
        private ClipTransition _CurrentAnimation;


        public override bool CanEnterState => true;

        public bool canCombo = false;
        private void OnEnable()
        {
            if (_CurrentAnimationIndex >= _Animations.Length - 1 ||
                _Animations[_CurrentAnimationIndex].State.Weight == 0)
            {
                _CurrentAnimationIndex = 0;
            }
            else
            {
                _CurrentAnimationIndex++;
            }
            if(_CurrentAnimationIndex > 1)
                {
                    if(!canCombo) _CurrentAnimationIndex = 0;
                }
            _CurrentAnimation = _Animations[_CurrentAnimationIndex];
            Character.Animancer.Play(_CurrentAnimation);
            Character.Parameters.ForwardSpeed = 0;
        }



        public override bool FullMovementControl => false;

        private void FixedUpdate()
        {
            if (Character.CheckMotionState())
                return;
            Character.Movement.TurnTowards((Target.position - transform.position).normalized, _TurnSpeed);
        }

        public override bool CanExitState
            => _CurrentAnimation.State.NormalizedTime >= _CurrentAnimation.State.Events.NormalizedEndTime;
    

    }