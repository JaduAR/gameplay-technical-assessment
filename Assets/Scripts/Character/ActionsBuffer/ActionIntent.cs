using UnityEngine;

namespace Game.Assets.Scripts.Character.ActionsBuffer
{
    public abstract class ActionIntent
    {
        private static ushort ID_COUNTER = 0;

        public ushort UniqueId { get; }

        protected ActionIntent()
        {
            UniqueId = ++ID_COUNTER;
        }
    }

    public class HandAttackIntent: ActionIntent
    {
    }

    public class MovementIntent: ActionIntent
    {
        public Vector2 Direction { get; }

        public MovementIntent(Vector2 direction) : base()
        {
            Direction = direction;
        }
    }
}
