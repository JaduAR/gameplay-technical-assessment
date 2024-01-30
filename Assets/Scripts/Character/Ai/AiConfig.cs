using Game.Assets.Scripts.EventSystem;
using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.Character.Ai
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Ai Config")]
    public class AiConfig: ScriptableObject
    {
        [SerializeField]
        [Range(0,1)]
        private float _avoidanceProbability;

        [SerializeField]
        private Vector2 _movementTime;

        [SerializeField]
        private Vector2 _movementSpeed;

        [SerializeField]
        private float _bashTime;

        [SerializeField]
        private GameEvent _attackStartEvent;

        [SerializeField]
        private GameEvent _attackCompleteEvent;


        public float AvoidanceProbability => _avoidanceProbability;
        public Vector2 MovementTime => _movementTime;
        public Vector2 MovementSpeed => _movementSpeed;
        public float BashTime => _bashTime;
        public GameEvent AttackStartEvent => _attackStartEvent;
        public GameEvent AttackCompleteEvent => _attackCompleteEvent;

        void OnValidate()
        {
            this.ValidateReference(_attackStartEvent);
            this.ValidateReference(_attackCompleteEvent);
        }
    }
}
