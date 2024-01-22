using UnityEngine;

namespace Game.Assets.Scripts.Utils
{
    public class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this as T;
            }
        }
    }
}
