using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Rigidbody rb;
    protected Animator animator;

    private int health = 100;
    public int Health => health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    public virtual void TakeDamage(int _damage, Transform _attackPos)
    {
        StartCoroutine(TakeDamageDelay(_damage, _attackPos));
    }

    private IEnumerator TakeDamageDelay(int _damage, Transform _attackPos)
    {
        yield return new WaitForSeconds(0.2f);

        health -= _damage;
        AttackManager.CreatePunchEffect(_attackPos);

        if (health <= 0)
        {
            GameManager.i.TriggerLevelEnd();
        }
    }

    
}