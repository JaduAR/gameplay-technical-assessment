
using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Character : MonoBehaviour
    {

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer => _Animancer;

        [SerializeField]
        private CharacterLocomotion _Movement;
        public CharacterLocomotion Movement => _Movement;

        [SerializeField]
        private ParamsCharacter _Parameters;
        public ParamsCharacter Parameters => _Parameters;

        [SerializeField]
        private StateBase.StateMachine _StateMachine;
        public StateBase.StateMachine StateMachine => _StateMachine;

        private void Awake()
            {
                StateMachine.InitializeAfterDeserialize();
            }
        public bool CheckMotionState()
            {
                StateBase state = Parameters.MovementDirection == default
                        ? StateMachine.DefaultState
                        : StateMachine.Locomotion;
    
                return
                    state != StateMachine.CurrentState &&
                    StateMachine.TryResetState(state);
            }
    }