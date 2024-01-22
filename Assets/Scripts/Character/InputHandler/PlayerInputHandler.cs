using Game.Assets.Scripts.Character.ActionsBuffer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Assets.Scripts.Character.InputHandler
{
    /// <summary>
    /// Connector between character controls and player input.
    /// </summary>
    public class PlayerInputHandler: MonoBehaviour
    {
        [SerializeField]
        private Character _targetCharacter;
    
        private IInputActionsBuffer _actionsBuffer;

        private IPlayerInputHandlerStrategy _moveHandlerStrategy;
        private IPlayerInputHandlerStrategy _attackHandlerStrategy;

        private void Start()
        {
            _actionsBuffer = _targetCharacter.ActionsBufferInput;

            _moveHandlerStrategy = new ContinuousActionPlayerInputHandlerStrategy(_actionsBuffer, 
                ctx => {
                    var value = ctx.ReadValue<Vector2>();
                    return new MovementIntent(value);
                });

            _attackHandlerStrategy = new ContinuousActionPlayerInputHandlerStrategy(_actionsBuffer, 
                ctx => {
                    return new HandAttackIntent();
                });
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveHandlerStrategy.Update(context);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            _attackHandlerStrategy.Update(context);
        }
    }
}