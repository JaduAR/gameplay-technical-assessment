using Game.Assets.Scripts.Character.Attacks;
using Game.Assets.Scripts.EventSystem;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Health
{
    [RequireComponent(typeof(Character))]
    public class HealthComponent : MonoBehaviour, IHealth
    {
        [SerializeField]
        private int _maxHealth;

        [SerializeField]
        private int _health;

        [SerializeField]
        private GameEvent _attackCompleteEvent;

        [SerializeField]
        private GameEvent _deathEvent;

        [SerializeField]
        private GameEvent _healthChangeEvent;

        private Character _character;

        public float Health => (float)_health / _maxHealth;

        private void OnValidate()
        {
            this.ValidateReference(_attackCompleteEvent);
        }

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void OnEnable()
        {
            _attackCompleteEvent.EventRaised += OnAttack;
        }

        private void OnDisable()
        {
            _attackCompleteEvent.EventRaised -= OnAttack;
        }

        private void OnAttack(Object sender, object data)
        {
            var attack = sender as Attack;
            var context = (AttackContext) data;

            if (attack != null && context.Target.Equals(_character))
            {
                ApplyDamage(attack.AttackPower);
            }
        }

        private void ApplyDamage(int damage)
        {
            if (damage <= 0 || _health == 0)
                return;

            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);

            _healthChangeEvent?.Raise(this, Health);

            if (_health == 0)
            {
                _deathEvent?.Raise(this, _character);
            }
        }
    }
}