using Animancer;
using Animancer.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateHeavyAttack : StateBase
    {
        [SerializeField] private ClipTransition chargeAttack;
        [SerializeField] private ClipTransition heavyAttack;


        public bool combo = false;
        public override bool CanEnterState => true;
        public override bool FullMovementControl => false;
        private void Awake()
            {
                chargeAttack.Events.OnEnd = HeavyAttack;
                heavyAttack.Events.OnEnd =  Character.StateMachine.ForceSetDefaultState;
            }
        void HeavyAttack()
            {
                Character.Animancer.Play(heavyAttack);
            }
        private void OnEnable()
            {
                Character.Animancer.Play(chargeAttack);
            }

        private void OnDisable()
            {
                combo = false;
            }
        private void FixedUpdate()
            {
                if (Character.CheckMotionState())
                    return;
              //  Character.Movement.TurnTowards((Target.position - transform.position).normalized, _TurnSpeed);
            }

        public override bool CanExitState
        => chargeAttack.State.NormalizedTime >= chargeAttack.State.Events.NormalizedEndTime;

    }
