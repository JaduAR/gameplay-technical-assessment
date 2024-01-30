using System;
using System.Threading.Tasks;

using UnityEngine;

namespace Game.Assets.Scripts.Character.ActionsBuffer
{
    // Facilitates the transformation of logic associated with two events — 
    // namely, the initiation and completion of an operation — into an asynchronous task that executes between these events.
    internal class ActionIntentAsyncExecutor<T>
        where T : ActionIntent
    {
        public event Action<T, Task<T>> OnExecute;

        private readonly IObservableActionsBuffer _actionsBuffer;
        private TaskCompletionSource<T> _taskCompletionSource;

        public ActionIntentAsyncExecutor(IObservableActionsBuffer actionsBuffer)
        {
            _actionsBuffer = actionsBuffer;

            _actionsBuffer.Subscribe<T>(ActionStart, ActionEnd);
        }

        private void ActionStart(T action)
        {
            if (_taskCompletionSource != null)
            {
                Debug.LogError($"{nameof(_taskCompletionSource)} supposed to be null");
                _taskCompletionSource.SetCanceled();
                _taskCompletionSource = null;
            }

            _taskCompletionSource = new TaskCompletionSource<T>();
            OnExecute?.Invoke(action, _taskCompletionSource.Task);
        }

        private void ActionEnd(T action)
        {
            if (_taskCompletionSource == null)
            {
                Debug.LogError($"{nameof(_taskCompletionSource)} not supposed to be null");
                return;
            }

            _taskCompletionSource.SetResult(action);
            _taskCompletionSource = null;
        }
    }
}