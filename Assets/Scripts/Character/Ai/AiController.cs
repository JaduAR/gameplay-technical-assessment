using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Ai
{
    [RequireComponent(typeof(Character))]
    public partial class AiController : MonoBehaviour
    {
        [SerializeField]
        private AiConfig _config;

        private Character _character;
        private FSM _stateMachine;

        void Awake()
        {
            _character = GetComponent<Character>();
        }

        void OnValidate()
        {
            this.ValidateReference(_config);
        }

        void OnEnable()
        {
            _stateMachine = new FSM();
            _stateMachine.AddState(new AvoidAttacksState(_stateMachine, _character, _config));
            _stateMachine.AddState(new BashedState(_stateMachine, _config));
        }

        void OnDisable()
        {
            _stateMachine.Dispose();
            _stateMachine = null;
        }

        void Update()
        {
            _stateMachine.Update();
        }
    }
}