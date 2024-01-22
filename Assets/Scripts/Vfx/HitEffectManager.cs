using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance;

    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private List<HitVfxBehavior> _vfxBehaviors = new List<HitVfxBehavior>();
    [SerializeField] private List<Transform> _positionPool = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        foreach (HitVfxBehavior hitVfxBehavior in _vfxBehaviors)
        {
            hitVfxBehavior.ResetEffect();
        }
    }

    public void ExecuteHitEffect()
    {
        var behavior = _vfxBehaviors[0];
        _vfxBehaviors.RemoveAt(0);
        behavior.StartEffect(_duration, GetRandomPosition());
    }

    public void SendToPool(HitVfxBehavior hitVfxBehavior)
    {
        _vfxBehaviors.Add(hitVfxBehavior);
    }

    private Vector3 GetRandomPosition()
    {
        int randomIndex = Random.Range(0, _positionPool.Count - 1);
        return _positionPool[randomIndex].position;
    }
}
