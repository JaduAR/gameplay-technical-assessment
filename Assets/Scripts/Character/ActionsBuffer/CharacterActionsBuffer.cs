using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Assets.Scripts.Character.ActionsBuffer
{
    public interface IInputActionsBuffer
    {
        void AddActionIntent(ActionIntent intent);
        void EndActionIntent(ActionIntent intent);
    }

    public interface IObservableActionsBuffer
    {
        void Subscribe<T>(Action<T> startHandler, Action<T> finishHandler) where T : ActionIntent;
        void UnSubscribe<T>(Action<T> startHandler, Action<T> finishHandler) where T : ActionIntent;

        void SubscribeToAsync<T>(Action<T, Task<T>> asyncHandler) where T : ActionIntent;
        void UnSubscribeFromAsync<T>(Action<T, Task<T>> asyncHandler) where T : ActionIntent;
    }

    // Serves as a centralized component for encapsulating input handling logic, orchestrating sequential execution of commands based on a specified strategy.
    // Designed to act as a unified point of control for managing character behavior, whether controlled by a player or AI.
    public class CharacterActionsBuffer: IObservableActionsBuffer, IInputActionsBuffer
    {
        private readonly Dictionary<Type, object> _asyncActionNotifiers = new ();
        private readonly ActionsBufferNotificationManager _notificationBehaviour;
        private readonly IActionsBufferStrategy _actionQueueStrategy;

        public CharacterActionsBuffer()
        {
            _notificationBehaviour = new ActionsBufferNotificationManager();
            _actionQueueStrategy = new SequentialActionsBufferStrategy();

            _actionQueueStrategy.OnActionExecutionStart += ActionExecutionStart;
            _actionQueueStrategy.OnActionExecutionEnd += ActionExecutionEnd;
        }

        public void Subscribe<T>(Action<T> startHandler, Action<T> finishHandler)
            where T : ActionIntent
        {
            _notificationBehaviour.Subscribe(startHandler, finishHandler);
        }

        public void UnSubscribe<T>(Action<T> startHandler, Action<T> finishHandler)
            where T : ActionIntent
        {
            _notificationBehaviour.UnSubscribe(startHandler, finishHandler);
        }

        public void AddActionIntent(ActionIntent intent)
        {
            _actionQueueStrategy.AddActionIntent(intent);
        }

        public void EndActionIntent(ActionIntent intent)
        {
            _actionQueueStrategy.EndActionIntent(intent);
        }

        public void SubscribeToAsync<T>(Action<T, Task<T>> asyncHandler) where T : ActionIntent
        {
            if (!_asyncActionNotifiers.TryGetValue(typeof(T), out var executor))
            {
                executor = new ActionIntentAsyncExecutor<T>(this);
                _asyncActionNotifiers.Add(typeof(T), executor);
            }
            var typedExecutor = (ActionIntentAsyncExecutor<T>)executor;
            typedExecutor.OnExecute += asyncHandler;
        }

        public void UnSubscribeFromAsync<T>(Action<T, Task<T>> asyncHandler) where T : ActionIntent
        {
            if (!_asyncActionNotifiers.TryGetValue(typeof(T), out var executor))
            {
                return;
            }
            var typedExecutor = (ActionIntentAsyncExecutor<T>)executor;
            typedExecutor.OnExecute -= asyncHandler;
        }

        private void ActionExecutionStart(ActionIntent intent)
        {
            _notificationBehaviour.NotifyOnStart(intent);
        }

        private void ActionExecutionEnd(ActionIntent intent)
        {
            _notificationBehaviour.NotifyOnEnd(intent);
        }
    }
}