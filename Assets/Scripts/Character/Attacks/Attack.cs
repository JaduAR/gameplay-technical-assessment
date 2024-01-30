using System;
using System.Threading.Tasks;
using Game.Assets.Scripts.EventSystem;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Attacks
{
    public abstract class Attack: ScriptableObject
    {
        [SerializeField]
        private string _animatorTriggerName;

        [SerializeField]
        private float _animationTime;

        [SerializeField]
        private float _range;

        [SerializeField]
        private int _power;

        [SerializeField]
        private GameEvent _attackStartedEvent;

        [SerializeField]
        private GameEvent _successfullAttackEvent;

        [SerializeField]
        private GameEvent _failedAttackEvent;

        public int AttackPower => _power;

        public async Task<bool> Execute(Task inputTrigger, AttackContext context)
        {
            context.Player.TriggerAnimation(_animatorTriggerName);
            _attackStartedEvent?.Raise(this, context);

            await WaitForAnimation(inputTrigger, context);

            var result = await PerformAttack(context);

            return result;
        }

        protected virtual Task WaitForAnimation(Task inputTrigger, AttackContext context)
        {
            return Task.Delay(TimeSpan.FromSeconds(_animationTime));
        }

        // TODO: Draft - Consider implementing the Command pattern to encapsulate attack calculation logic,
        // facilitating the use of physics or other mechanisms for improved structure and flexibility.
        protected virtual Task<bool> PerformAttack(AttackContext context)
        {
            if (context.Target != null)
            {
                var distance = context.Player.Position - context.Target.Position;

                if (distance.magnitude <= _range)
                {
                    _successfullAttackEvent?.Raise(this, context);
                    return Task.FromResult(true);
                }
            }

            _failedAttackEvent?.Raise(this, context);
            return Task.FromResult(false);
        }
    }
}