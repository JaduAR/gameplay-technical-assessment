using UnityEngine;

namespace Game.Assets.Scripts.Character.Movement
{
    public interface IMovementStrategy
    {
        Vector2 Execute(Vector2 input);
    }
}