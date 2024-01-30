using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Assets.Scripts.Character.ActionsBuffer;
using Game.Assets.Scripts.Character.Attacks;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character
{
    [RequireComponent(typeof(ICharacter))]
    public class AttackComponent: MonoBehaviour
    {
        [SerializeField]
        private AttackSequence[] _handAttackSequences;

        private IObservableActionsBuffer _actionsBuffer;
        private ICharacter _character;
        private Animator _animator;

        private IEnumerator<Attack> _sequenceEnumerator;
        private int _sequenceIndex = 0;
        private bool _isExecuting = false;

        private void OnValidate()
        {
            this.ValidateNotEmpty(_handAttackSequences);
        }

        private void Awake()
        {
            _character = GetComponent<ICharacter>();
            _animator = GetComponent<Animator>();
            _actionsBuffer = _character.ActionsBuffer;
        }

        private void OnEnable()
        {
            _actionsBuffer.SubscribeToAsync<HandAttackIntent>(ActionStart);
            _sequenceEnumerator = _handAttackSequences[_sequenceIndex].GetAttacks();
            _sequenceEnumerator.MoveNext();
        }

        private void OnDisable()
        {
            _actionsBuffer.UnSubscribeFromAsync<HandAttackIntent>(ActionStart);
        }

        private async void ActionStart(HandAttackIntent intent, Task<HandAttackIntent> task)
        {
            if (_isExecuting)
                return;

            _isExecuting = true;

            var sequence = _handAttackSequences[_sequenceIndex];
            var attack = _sequenceEnumerator.Current;
        
            // TODO encapsulate this call in attack logic
            // I wanted to preserve RootMotion for attacks
            // But for movement across the map if works not really well because of different movement speed for axes
            _animator.applyRootMotion = true;

            var isSuccessful = await attack.Execute(task, new AttackContext(_character, _character.Target));

            _animator.applyRootMotion = false;

            if (!isSuccessful || !_sequenceEnumerator.MoveNext())
            {
                _sequenceIndex = ++_sequenceIndex % _handAttackSequences.Length;
                _sequenceEnumerator = _handAttackSequences[_sequenceIndex].GetAttacks();
                _sequenceEnumerator.MoveNext();
            }

            await task;
            _isExecuting = false;
        }
    }
}