using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Assets.Scripts.Character.ActionsBuffer
{
    // Encapsulates publish/subscribe logic, providing a generic interface for event management.
    internal class ActionsBufferNotificationManager
    {
        private readonly Dictionary<Type, IActionIntentNotifier> _notifiers = new ()
        {
            { typeof(MovementIntent), new ActionIntentObservableNotifier<MovementIntent>() },
            { typeof(HandAttackIntent), new ActionIntentObservableNotifier<HandAttackIntent>() },
        };        

        public void Subscribe<T>(Action<T> startHandler, Action<T> finishHandler)
            where T : ActionIntent
        {
            var notifier = (ActionIntentObservableNotifier<T>) GetNotifier(typeof(T));
            notifier.OnStart += startHandler;
            notifier.OnFinish += finishHandler;
        }

        public void UnSubscribe<T>(Action<T> startHandler, Action<T> finishHandler)
            where T : ActionIntent
        {
            var notifier = (ActionIntentObservableNotifier<T>)GetNotifier(typeof(T));
            notifier.OnStart -= startHandler;
            notifier.OnFinish -= finishHandler;
        }

        public void NotifyOnStart(ActionIntent intent)
        {
            var notifier = GetNotifier(intent.GetType());
            notifier.NotifyStart(intent);
        }

        public void NotifyOnEnd(ActionIntent intent)
        {
            var notifier = GetNotifier(intent.GetType());
            notifier.NotifyFinish(intent);
        }

        private IActionIntentNotifier GetNotifier(Type type)
        {
            if (!_notifiers.TryGetValue(type, out var notifier))
            {
                Debug.LogError($"Notifier not found for type {type}");
                return null;
            }

            return notifier;
        }

        private interface IActionIntentNotifier
        {
            void NotifyStart(ActionIntent intent);
            void NotifyFinish(ActionIntent intent);
        }

        private class ActionIntentObservableNotifier<T>: IActionIntentNotifier
            where T : ActionIntent
        {
            public event Action<T> OnStart;
            public event Action<T> OnFinish;

            public void NotifyStart(ActionIntent intent)
            {
                var typedIntent = intent as T;

                OnStart?.Invoke(typedIntent);
            }

            public void NotifyFinish(ActionIntent intent)
            {
                var typedIntent = intent as T;

                OnFinish?.Invoke(typedIntent);
            }
        }
    }
}
