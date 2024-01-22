using System;
using Game.Assets.Scripts.Character.ActionsBuffer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Assets.Scripts.Character.InputHandler
{
    public interface IPlayerInputHandlerStrategy
    {
        void Update(InputAction.CallbackContext context);
    }

    internal class ContinuousActionPlayerInputHandlerStrategy: IPlayerInputHandlerStrategy
    {
        private readonly IInputActionsBuffer _actionsBuffer;
        private readonly Func<InputAction.CallbackContext, ActionIntent> _intentFactory;

        private ActionIntent _intent;

        public ContinuousActionPlayerInputHandlerStrategy(IInputActionsBuffer actionsBuffer, Func<InputAction.CallbackContext, ActionIntent> intentFactory)
        {
            _actionsBuffer = actionsBuffer;
            _intentFactory = intentFactory;
        }

        public void Update(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Waiting:
                    break;

                case InputActionPhase.Performed:

                    if (_intent != null)
                    {
                        // It's ok. Happens when same type of input happens at the same time.
                        // e.g. Another movement button pressed in addition to the already pressed one.
                        _actionsBuffer.EndActionIntent(_intent);
                        _intent = null;
                    }

                    _intent = _intentFactory(context);
                    _actionsBuffer.AddActionIntent(_intent);

                    break;

                case InputActionPhase.Canceled:
                    if (_intent == null)
                    {
                        Debug.LogError($"{nameof(_intent)} is null!");
                        break;
                    }
                    _actionsBuffer.EndActionIntent(_intent);
                    _intent = null;

                    break;

                default:
                    if (_intent != null)
                    {
                        Debug.LogError($"{nameof(_intent)} is not null!");

                        _actionsBuffer.EndActionIntent(_intent);
                        _intent = null;
                    }
                    break;
            }
        }
    }
}