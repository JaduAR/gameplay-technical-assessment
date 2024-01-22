using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAI : MonoBehaviour
    {
    public Transform opponent;
    public float farDist = 2;
    Animator animator;

    float timer;
    Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
        {
        animator = GetComponent<Animator>();

        ResetTimer();
        }

    void ResetTimer()
        {
        timer = Random.Range(1.0f, 3.5f);

        if (Random.Range(0, 10) < 3)
            {
            moveDir = Vector2.zero;
            return;
            }

        Vector3 dirToOpp = transform.position - opponent.position;
        if (dirToOpp.sqrMagnitude > farDist)
            {
            // move towards player
            moveDir = new Vector2(0,1);
            return;
            }


        moveDir = new Vector2(Random.Range(-1.0f, 1.0f),
                              Random.Range(-1.0f, 1.0f));
        }

    // Update is called once per frame
    void Update()
        {
        timer -= Time.deltaTime;
        if (timer < 0)
            ResetTimer();

        animator.SetFloat("StrafeX", moveDir.x);
        animator.SetFloat("StrafeZ", moveDir.y);
        }
    }
