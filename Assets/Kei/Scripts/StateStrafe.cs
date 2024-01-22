using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStrafe : StateBase
{

    [SerializeField]
    private MixerTransition2D _Strafe;

    [SerializeField, Range(0,1)]
    private float _MovementLevel;

    private LinearMixerState _movementMixer;
    [SerializeField]
    private ClipTransition _Idle;
    public ClipTransition Idle => _Idle;


    public override bool CanEnterState => true;

    private void Awake()
        {
            _movementMixer = new LinearMixerState();
            
          //  Character.Animancer.Play(CurrentMixer);
          Character.Animancer.Play(_Strafe);
        }
    private void OnEnable()
        {
            Character.Animancer.Play(_Strafe);
        }

    private void FixedUpdate()
        {
      //  Character.Animancer.Play(_Strafe);

        
            if (Character.CheckMotionState())
                return;
              //  Character.Movement.UpdateSpeedControl();
                _Strafe.State.Parameter = new Vector2(Character.Parameters.MovementDirection.x, Character.Parameters.MovementDirection.z);
                UpdateRotation();
        

        }
    private void UpdateRotation()
        {
            Character.Movement.TurnTowards((Target.position - transform.position).normalized, 400);
        }
    }
