using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{   
    [SerializeField] private PlayerController _playerController;

    public float _currentHealth;
    public float _maxHealth { get; private set; } = 100;    
    public bool _attackReceived { get; set; }
    public int _amountAttacked { get; set; }

    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && _playerController._currentState == AttackState.Attacking)
            OnAttackReceived();
    }

    private void OnAttackReceived()
    {
        if (_attackReceived)
            return;

        Debug.Log("attack received");
        _attackReceived = true;
        _amountAttacked++;       
    }

    public void TakeDamage(float amount, string damageAnim)
    {
        if (!_attackReceived)
            return;

        _animator.SetTrigger(damageAnim);

        _currentHealth -= amount;
        if(_currentHealth <= 0)
        {
            Debug.Log("Enemy defeated!");
            _currentHealth = 0;
        }

        _attackReceived = false;
    }
}
