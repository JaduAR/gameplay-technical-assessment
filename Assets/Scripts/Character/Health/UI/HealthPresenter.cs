using Game.Assets.Scripts.EventSystem;
using Game.Assets.Scripts.Managers;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Health.UI
{
    public class HealthPresenter : MonoBehaviour
    {
        [SerializeField]
        private HealthBarView _healthBarView;

        [SerializeField]
        private GameEvent _healthChangedEvent;

        [SerializeField]
        private GameEvent _targetChangedEvent;

        private IHealth _healthComponent;

        private void OnValidate()
        {
            this.ValidateReference(_healthBarView);
            this.ValidateReference(_healthChangedEvent);
            this.ValidateReference(_targetChangedEvent);
        }

        private void Start()
        {
            _healthComponent = GameRoot.Instance.Player.Target?.Health;
            RefreshState();
        }

        private void OnEnable()
        {
            _healthChangedEvent.EventRaised += OnHealthChanged;
            _targetChangedEvent.EventRaised += OnTargetChanged;
        }

        private void OnDisable()
        {
            _healthChangedEvent.EventRaised -= OnHealthChanged;
            _targetChangedEvent.EventRaised -= OnTargetChanged;
        }

        private void OnHealthChanged(Object sender, object data)
        {
            if (!ReferenceEquals(sender, _healthComponent))
            {
                return;
            }

            RefreshState();
        }

        private void OnTargetChanged(Object sender, object data)
        {
            var target = data as ICharacter;
            if (target == null)
            {
                _healthComponent = null;
            }
            else
            {
                _healthComponent = target.Health;
            }
            RefreshState();
        }

        private void RefreshState()
        {
            if (_healthComponent != null)
            {
                _healthBarView.SetState(HealthBarView.State.HealthBar);
                _healthBarView.SetHealth(_healthComponent.Health);
            }
            else
            {
                _healthBarView.SetState(HealthBarView.State.NoTarget);
            }
        }
    }
}
