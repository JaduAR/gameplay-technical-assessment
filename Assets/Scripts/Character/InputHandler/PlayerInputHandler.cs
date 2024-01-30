using Game.Assets.Scripts.Character.ActionsBuffer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Game.Assets.Scripts.Character.InputHandler
{
    /// <summary>
    /// Connector between character controls and player input.
    /// </summary>
    [RequireComponent(typeof(Character))]
    public class PlayerInputHandler: MonoBehaviour, InputActions.IPlayerActions
    {
        private InputActions _input;

        private IInputActionsBuffer _actionsBuffer;

        private IPlayerInputHandlerStrategy _moveHandlerStrategy;
        private IPlayerInputHandlerStrategy _attackHandlerStrategy;

        private void Awake()
        {
            _actionsBuffer = GetComponent<Character>().ActionsBufferInput;

            _moveHandlerStrategy = new ContinuousActionPlayerInputHandlerStrategy(_actionsBuffer,
                ctx => {
                    var value = ctx.ReadValue<Vector2>();
                    return new MovementIntent(value);
                });

            _attackHandlerStrategy = new ContinuousActionPlayerInputHandlerStrategy(_actionsBuffer,
                ctx => new HandAttackIntent());

            _input = new InputActions();
            _input.Player.SetCallbacks(this);
        }

        private void OnEnable()
        {
            _input.Player.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveHandlerStrategy.Update(context);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            _attackHandlerStrategy.Update(context);
        }
    }
}