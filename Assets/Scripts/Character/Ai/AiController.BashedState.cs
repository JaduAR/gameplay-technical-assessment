using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Ai
{
    public partial class AiController
    {
        private class BashedState: FSMState
        {
            private readonly AiConfig _config;
            private float _delayTime;

            public BashedState(FSM stateMachine, AiConfig config) : base(stateMachine)
            {
                _config = config;
            }

            public override void Enter()
            {
                _delayTime = _config.BashTime;
            }

            public override FSMState Update()
            {
                if (_delayTime > 0)
                {
                    _delayTime -= Time.deltaTime;
                    return this;
                }

                return GetState<AvoidAttacksState>();
            }
        }
    }
}