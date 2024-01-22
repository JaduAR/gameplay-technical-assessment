using UnityEngine;

#pragma warning disable 114
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;

    public static T Instance {
        get {
            if (instance != null) {
                return instance;
            }

            return null;
        }
    }

    protected virtual void Awake() {
        if (instance == null) {
            instance = this as T;
        }
        else {
            Destroy(gameObject);
        }
    }
}

#pragma warning restore  114