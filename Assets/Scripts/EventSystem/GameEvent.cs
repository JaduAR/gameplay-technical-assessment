using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Assets.Scripts.EventSystem
{
    [CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
    public class GameEvent: ScriptableObject
    {
        public event Action<Object, object> EventRaised;

        public void Raise(Object sender, object data)
        {
            EventRaised?.Invoke(sender, data);
        }
    }
}
