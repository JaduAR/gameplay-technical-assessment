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

    public virtual void TakeDamage(int _damage)
    {
        health -= _damage;

        if(health <= 0)
        {
            Debug.Log("Dead");
        }
    }
}