using Game.Assets.Scripts.Utils;
using UnityEngine;

namespace Game.Assets.Scripts.EventSystem
{
    public class GameEventListener: MonoBehaviour
    {
        [SerializeField]
        private GameEvent _gameEvent;

        [SerializeField]
        private GameUnityEvent _action;

        private void OnValidate()
        {
            this.ValidateReference(_gameEvent);
            this.ValidateReference(_action);
        }

        private void OnEnable()
        {
            _gameEvent.EventRaised += OnEventRaised;
        }

        private void OnDisable()
        {
            _gameEvent.EventRaised -= OnEventRaised;
        }

        public void OnEventRaised(Object component, object data)
        {
            _action.Invoke(component, data);
        }
    }
}