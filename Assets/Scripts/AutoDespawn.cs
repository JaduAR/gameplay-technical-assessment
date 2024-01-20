using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField]
    private float _timeAlive = 1.0f;

    private float _currentTimer = 0.0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        _currentTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= _timeAlive)
        {
            PoolManager.Instance.Despawn(transform);
        }
    }
}
