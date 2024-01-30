using System.Collections;
using Game.Assets.Scripts.Character.ActionsBuffer;
using Game.Assets.Scripts.Character.Attacks;
using Game.Assets.Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.Assets.Scripts.Character.Ai
{
    public partial class AiController
    {
        private class AvoidAttacksState: FSMState
        {
            private readonly Character _character;
            private readonly IInputActionsBuffer _actionsBuffer;
            private readonly AiConfig _config;
            private Coroutine _avoidanceCoroutine;

            private bool _requestExit = false;

            public AvoidAttacksState(FSM stateMachine, Character character, AiConfig config) : base(stateMachine)
            {
                _character = character;
                _actionsBuffer = character.ActionsBufferInput;
                _config = config;
            }

            public override void Enter()
            {
                _requestExit = false;
                _config.AttackStartEvent.EventRaised += OnAttackStarted;
                _config.AttackCompleteEvent.EventRaised += OnAttackComplete;
            }

            public override void Exit()
            {
                _requestExit = true;
                _config.AttackStartEvent.EventRaised -= OnAttackStarted;
                _config.AttackCompleteEvent.EventRaised -= OnAttackComplete;
            }

            public override void Dispose()
            {
                if (_avoidanceCoroutine != null)
                {
                    _character.StopCoroutine(_avoidanceCoroutine);
                    _avoidanceCoroutine = null;
                }
            }

            private void OnAttackStarted(Object sender, object data)
            {
                var context = (AttackContext)data;

                if (!_character.Equals(context.Target))
                    return;

                if (Random.value > _config.AvoidanceProbability || _avoidanceCoroutine != null)
                    return;

                if (Vector3.Distance(_character.Position, context.Player.Position) > 3)
                    return;

                _avoidanceCoroutine = _character.StartCoroutine(AvoidanceCoroutine(context));
            }

            private void OnAttackComplete(Object sender, object data)
            {
                var context = (AttackContext)data;

                if (!_character.Equals(context.Target))
                    return;

                // Stop movement and switch to bashed state
                _requestExit = true;
            }

            private IEnumerator AvoidanceCoroutine(AttackContext context)
            {
                var speed = Random.Range(_config.MovementSpeed.x, _config.MovementSpeed.y);

                var intent = new MovementIntent(new Vector2(speed * Random.value * Random.Range(-1, 2), -speed));

                _actionsBuffer.AddActionIntent(intent);
                var movementTime = Random.Range(_config.MovementTime.x, _config.MovementTime.y);

                while (movementTime > 0 && !_requestExit)
                {
                    movementTime -= Time.deltaTime;
                    yield return null;
                }

                _actionsBuffer.EndActionIntent(intent);

                _avoidanceCoroutine = null;
            }


            public override FSMState Update()
            {
                if (_requestExit)
                {
                    return GetState<BashedState>();
                }

                return this;
            }
        }
    }
}