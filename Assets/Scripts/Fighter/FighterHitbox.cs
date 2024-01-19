using UnityEngine;

/// <summary>
/// Responsible for detecting collisions between a fighter's hitbox and opponents.
/// </summary>
public class FighterHitbox : MonoBehaviour
{
    [Tooltip("Layer mask for detecting triggers.")]
    [SerializeField] private LayerMask _fighterLayerMask;

    [Tooltip("Fighter who has this hitbox.")]
    [SerializeField] private Fighter _fighter;

    private void OnValidate()
    {
        if (_fighter == null)
            Debug.LogError($"Fighter was not assigned to fighter hitbox {gameObject.name}.", this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _fighterLayerMask) == 0) return;
        var targetHealth = other.GetComponent<IHasHealth>();
        targetHealth?.TakeDamage(_fighter.CurrentDamageRate);
        _fighter.PunchLanded();
    }
}