using Animancer.Units;
using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateIdle : StateBase
    {

        [SerializeField] ClipTransition mainAnim;
        public override bool CanEnterState => true;
        private void OnEnable()
            {
                Character.Animancer.Play(mainAnim);
            }

        private void FixedUpdate()
            {
                if (Character.CheckMotionState()) return;

                Character.Movement.UpdateSpeedControl();
                Character.Animancer.Play(mainAnim);

                if(Target)
                Character.Movement.TurnTowards((Target.position - transform.position).normalized, 400);
            }
    }
