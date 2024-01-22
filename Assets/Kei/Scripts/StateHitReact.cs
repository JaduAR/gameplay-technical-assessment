using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHitReact : StateBase
    {
        [SerializeField] private ClipTransition _Animation;
        [SerializeField] private ClipTransition _AnimationHarder;
        private ClipTransition currentAnim;
        private void Awake()
            { 
                Character.Animancer.Play(_Animation);
                _Animation.Events.OnEnd = Character.StateMachine.ForceSetDefaultState;
                _AnimationHarder.Events.OnEnd = Character.StateMachine.ForceSetDefaultState;
            }
        public void OnDamageReceived(int _type)
            {
                currentAnim = _type == 0 ? _Animation : _AnimationHarder;
                Character.Animancer.Play(currentAnim, 0.25f, FadeMode.FromStart);
                Character.StateMachine.ForceSetState(this);
            }
        private void OnEnable()
            {
                Character.Animancer.Play(currentAnim, 0.25f, FadeMode.FromStart);
            }
        public override bool FullMovementControl => false;
        public override bool CanExitState => true;
    }
