using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    Idle,
    MoveAwayPlayer,
    MoveTowardsPlayer
}

public class Opponent : Character
{
    private EnemyState currentState = EnemyState.Idle;

    private void Start()
    {
        StartCoroutine(StartStateSwitchTimer());
    }

    private void Update()
    {
        transform.LookAt(GameManager.i.player.transform);

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetFloat("StrafeZ", 0);
                break;
            case EnemyState.MoveAwayPlayer:
                animator.SetFloat("StrafeZ", -0.5f);
                break;
            case EnemyState.MoveTowardsPlayer:
                animator.SetFloat("StrafeZ", 0.5f);
                break;
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        GameManager.i.uiManager.UpdateOpponentHealth();
    }

    private void SetState(EnemyState newState)
    {
        currentState = newState;

        StartCoroutine(StartStateSwitchTimer());
    }


    private IEnumerator StartStateSwitchTimer()
    {
        var waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime); //we will make a random range

        var randomState = Random.Range(0, System.Enum.GetValues(typeof(EnemyState)).Length);

        SetState((EnemyState)randomState);
    }
}
