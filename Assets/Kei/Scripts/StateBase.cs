using Animancer.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public abstract class StateBase : StateBehaviour, IOwnedState<StateBase>
    {

        [System.Serializable]
            public class StateMachine : StateMachine<StateBase>.WithDefault
            {

                [SerializeField]
                private StateBase _Locomotion;
                public StateBase Locomotion => _Locomotion;

                [SerializeField]
                private StateBase _Airborne;
                public StateBase Airborne => _Airborne;

            
            }


        [SerializeField]
        private Character _Character;
        public Character Character => _Character;
        [SerializeField]
        private Transform target;
        public Transform Target => target;
        public StateMachine<StateBase> OwnerStateMachine => _Character.StateMachine;
        public virtual bool StickToGround => true;
        public virtual Vector3 RootMotion => _Character.Animancer.Animator.deltaPosition;
        public virtual bool FullMovementControl => true;
    }