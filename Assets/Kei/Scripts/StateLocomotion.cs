using Animancer;
using Animancer.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLocomotion : StateBase
{

    [SerializeField] private LinearMixerTransition _LocomotionMixer;
    [SerializeField, DegreesPerSecond] private float _TurnSpeed = 400;
    public override bool CanEnterState => true;
    private void Awake()
        {
            Character.Animancer.Play(_LocomotionMixer);

        }
    private void OnEnable()
        {
            Character.Animancer.Play(_LocomotionMixer);
        }

       private void FixedUpdate()
        {
            if (Character.CheckMotionState())
                return;

            Character.Movement.UpdateSpeedControl();
            _LocomotionMixer.State.Parameter = 2;
            UpdateRotation();

        }
        private void UpdateRotation()
        {
            Character.Movement.TurnTowards((Target.position - transform.position).normalized, _TurnSpeed);
        }
    }
