using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Assets.Scripts.Utils
{
    public class FSM : IDisposable
    {
        private readonly Dictionary<Type, FSMState> _states = new ();

        private FSMState _defaultState;
        private FSMState _currentState;
        private float _lastUpdateTime;

        public void AddState<T>(T state)
            where T : FSMState
        {
            _states.Add(typeof(T), state);

            if (_defaultState == null)
                _defaultState = state;
        }

        public void Update()
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
                _defaultState.Enter();
                return;
            }

            var nextState = _currentState.Update();

            if (nextState != _currentState)
            {
                _currentState.Exit();
                _currentState = nextState;
                _currentState.Enter();
            }
        }

        public T GetState<T>()
            where T : FSMState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            {
                return (T)state;
            }

            Debug.LogError($"State {typeof(T)} is absent in {nameof(FSM)}");
            return null;
        }

        public void Dispose()
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            foreach (var state in _states.Values)
            {
                state.Dispose();
            }
        }
    }

    public abstract class FSMState : IDisposable
    {
        private readonly FSM _stateMachine;

        protected FSMState(FSM stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected FSMState GetState<T>()
            where T : FSMState
        {
            return _stateMachine.GetState<T>();
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public abstract FSMState Update();
        public virtual void Dispose()
        {
        }
    }
}
