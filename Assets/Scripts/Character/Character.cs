using Game.Assets.Scripts.Character.ActionsBuffer;
using Game.Assets.Scripts.Character.Health;
using Game.Assets.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Assets.Scripts.Character
{
    public interface ICharacter
    {
        [CanBeNull] ICharacter Target { get; }
        [CanBeNull] IHealth Health { get; }
        IObservableActionsBuffer ActionsBuffer { get; }
        Vector3 Position {  get; }
        void TriggerAnimation(string triggerName);
    }

    [RequireComponent(typeof(Animator), typeof(ITargetSelector))]
    public class Character: MonoBehaviour, ICharacter
    {
        [SerializeField]
        private GameObject _bodyCenter;

        private readonly CharacterActionsBuffer _actionsBuffer = new();

        private Animator _animator;
        private ITargetSelector _targetSelector;

        public IInputActionsBuffer ActionsBufferInput => _actionsBuffer;
        public IObservableActionsBuffer ActionsBuffer => _actionsBuffer;
        public ICharacter Target => _targetSelector.Target;
        public Vector3 Position => _bodyCenter.transform.position;
        public IHealth Health => GetComponent<HealthComponent>();
    

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _targetSelector = GetComponent<ITargetSelector>();
        }

        private void OnValidate()
        {
            this.ValidateReference(_bodyCenter);
        }

        public void TriggerAnimation(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }
    }
}