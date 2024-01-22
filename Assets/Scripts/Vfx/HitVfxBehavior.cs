using UnityEngine;

public class HitVfxBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private float _duration = 0f;
    private bool _start = false;

    private void Update()
    {
        if (!_start)
            return;

        _duration -= Time.deltaTime;

        if (_duration < 0f)
            EndEffect();
    }

    public void ResetEffect()
    {
        _start = false;
        _spriteRenderer.enabled = false;
    }

    public void StartEffect(float effectDuration, Vector3 effectPos)
    {
        _duration = effectDuration;
        transform.position = effectPos;
        _spriteRenderer.enabled = true;
        _start = true;
    }
    
    private void EndEffect()
    {
        ResetEffect();
        HitEffectManager.Instance.SendToPool(this);
    }
}
