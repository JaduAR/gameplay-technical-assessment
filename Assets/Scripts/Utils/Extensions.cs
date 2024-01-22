using System.Collections;

using UnityEngine;

namespace Game.Assets.Scripts.Utils
{
    public static class Extensions
    {
        public static bool IsInsignificant(this float value)
        {
            const float tolerance = 1e-6f;

            return Mathf.Abs(value) < tolerance;
        }

        public static void ValidateReference<T>(this Object context, T reference)
        {
            if (reference == null)
            {
                Debug.LogError($"Reference {typeof(T)} not set for {context.GetType()}", context);
            }
        }

        public static void ValidateNotEmpty<T>(this Object context, T reference)
            where T: ICollection
        {
            if (reference == null || reference.Count == 0)
            {
                Debug.LogError($"Collection {typeof(T)} is empty for {context.GetType()}", context);
            }
        }
    }
}
