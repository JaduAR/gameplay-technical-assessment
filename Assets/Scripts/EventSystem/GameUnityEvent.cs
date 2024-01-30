using System;
using UnityEngine.Events;

namespace Game.Assets.Scripts.EventSystem
{
    [Serializable]
    public class GameUnityEvent: UnityEvent<UnityEngine.Object, object> { }
}
