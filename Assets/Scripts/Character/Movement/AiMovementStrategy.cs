using UnityEngine;

namespace Game.Assets.Scripts.Character.Movement
{
    public class AiMovementStrategy: IMovementStrategy
    {
        public Vector2 Execute(Vector2 direction)
        {
            return direction;
        }
    }
}