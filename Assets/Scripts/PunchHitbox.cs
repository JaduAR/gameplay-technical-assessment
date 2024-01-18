using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Collider script to represent the hitbox around player's fists. Only is enabled while punching.
public class PunchHitbox : MonoBehaviour
{
    [SerializeField] private Collider _hitboxCollider;

    private float _damageToDealOnHit = 10;

    //When the hitbox is enabled, also pass how much damage this attack should do.
    public void EnableHitbox(float damage)
    {
        _hitboxCollider.enabled = true;
        _damageToDealOnHit = damage;
    }

    public void DisableHitbox()
    {
        if (_hitboxCollider.enabled)
        {
            //If the hitbox is still enabled, player missed, so end combo
            GameManager.Instance.ResetCombo();
            _hitboxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            AudioManager.Instance.PlayPunchAudio();
            GameManager.Instance.TakeDamage(_damageToDealOnHit);
            _hitboxCollider.enabled = false;
        }
    }
}
