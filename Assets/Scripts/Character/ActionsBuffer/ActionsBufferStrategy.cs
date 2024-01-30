using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Assets.Scripts.Character.ActionsBuffer
{
    internal interface IActionsBufferStrategy
    {
        event Action<ActionIntent> OnActionExecutionStart;
        event Action<ActionIntent> OnActionExecutionEnd;

        void AddActionIntent(ActionIntent intent);
        void EndActionIntent(ActionIntent intent);
    }

    internal class SequentialActionsBufferStrategy: IActionsBufferStrategy
    {
        public event Action<ActionIntent> OnActionExecutionStart;
        public event Action<ActionIntent> OnActionExecutionEnd;

        private readonly Queue<ushort> _intentsQueue = new Queue<ushort>();
        private readonly Dictionary<ushort, ActionIntent> _intents = new Dictionary<ushort, ActionIntent>();

        private ActionIntent _activeIntent;

        public void AddActionIntent(ActionIntent intent)
        {
            if (!_intents.TryAdd(intent.UniqueId, intent))
            {
                Debug.LogError($"Intent {intent.UniqueId} already exists!");
                return;
            }
            _intentsQueue.Enqueue(intent.UniqueId);

            UpdateQueue();
        }

        public void EndActionIntent(ActionIntent intent)
        {
            if (intent.UniqueId == _activeIntent.UniqueId)
            {
                SetActiveIntent(null);
            }

            if (!_intents.ContainsKey(intent.UniqueId))
            {
                Debug.LogError($"Intent {intent.UniqueId} is not exists!");
                return;
            }
            _intents.Remove(intent.UniqueId);

            UpdateQueue();
        }

        private void SetActiveIntent(ActionIntent intent)
        {
            if (_activeIntent != null)
            {
                OnActionExecutionEnd?.Invoke(_activeIntent);
                _activeIntent = null;
            }

            if (intent != null)
            {
                _activeIntent = intent;
                OnActionExecutionStart?.Invoke(_activeIntent);
            }
        }

        private void UpdateQueue()
        {
            if (_activeIntent != null)
                return;

            // Remove canceled intents from the action queue that were canceled before their scheduled execution.
            while (_intentsQueue.TryPeek(out var id) && !_intents.ContainsKey(id))
            {
                _intentsQueue.Dequeue();
            }

            if (_intentsQueue.TryDequeue(out var activeIntentId))
            {
                if (!_intents.TryGetValue(activeIntentId, out var intent))
                {
                    Debug.LogError($"Intent {activeIntentId} not found!");
                }

                SetActiveIntent(intent);
            }
        }
    }
}
