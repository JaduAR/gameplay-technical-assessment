using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this.GetComponent<T>();
        else
            Destroy(this.gameObject);
    }
}
